using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static BuildingBase;

public class PlayerProgress : MonoBehaviour
{
    [SerializeField] private ProgressLine progressLine;
    [SerializeField] private float progressRange;
    private BuildingBase progressTarget;

    private void Update()
    {
        Progress();
    }

    private void Progress()
    {
        Collider2D[] within = Physics2D.OverlapCircleAll(transform.position, progressRange, LayerMask.GetMask("Building"));

        if (within.Length == 0)
        {
            progressTarget = null;
            progressLine.LineDelete();
            return;
        }

        List<BuildingBase> candidates = within
            .Select(hitCol => MapManager.instance.GetBuilding(Vector3Int.RoundToInt(hitCol.gameObject.transform.position)))
            .Where(building => building != null &&
                  (building.buildState == BuildState.Build || building.buildState == BuildState.Destroy) &&
                   CheckCosts(building))
            .ToList();

        if (candidates.Count == 0)
        {
            progressTarget = null;
            progressLine.LineDelete();
            return;
        }

        if (progressTarget != null && candidates.Contains(progressTarget))
        {
            progressTarget.Progress();
            AudioManager.instance.PlayBuildBGS();
            progressLine.LineDraw(progressTarget);
            return;
        }

        float closestDistance = float.MaxValue;

        foreach (BuildingBase building in candidates)
        {
            if (!building) continue;

            float distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                progressTarget = building;
            }
        }

        if (progressTarget)
        {
            progressTarget.Progress();
            AudioManager.instance.PlayBuildBGS();
            progressLine.LineDraw(progressTarget);
        }
        progressLine.LineDelete();
    }
    public void ChangeTarget(BuildingBase nextTarget)
    {
        progressTarget = nextTarget;
    }
    private bool CheckCosts(BuildingBase buildTarget)
    {
        BuildCost[] costs = buildTarget.data.currentBuildingCosts;
        if (buildTarget.buildState == BuildState.Destroy)
            return true;
        for (int i = 0; i < costs.Length; i++)
            if (!ResourceManager.instance.CheckResourceNum(costs[i].resource, (int)(costs[i].amount / buildTarget.data.currentBuildTime * Time.deltaTime)))
                return false;
        return true;
    }
}
