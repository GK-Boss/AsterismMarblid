using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCostPanel : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject prefab;

    BuildingData generated;
    List<GameObject> costs = new();
    public void GenerateCostUI(BuildingData data)
    {
        if (data == generated)
            return;

        foreach (var cost in costs)
        {
            Destroy(cost.gameObject);
        }
        costs.Clear();
        foreach (var cost in data.currentBuildingCosts)
        {
            GameObject gene = Instantiate(prefab, content);
            costs.Add(gene);
            gene.GetComponent<BuildingCostPrefab>().Init(cost);
        }
        generated = data;
    }
}
