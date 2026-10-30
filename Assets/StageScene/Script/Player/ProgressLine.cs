using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static BuildingBase;

public class ProgressLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void LineDraw(BuildingBase buildTarget)
    {
        if (buildTarget)
        {
            lineRenderer.enabled = true;
            if (buildTarget.buildState == BuildState.Build)
            {
                lineRenderer.startColor = Color.yellow;
                lineRenderer.endColor = Color.yellow;
            }
            else if (buildTarget.buildState == BuildState.Destroy)
            {
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }
            int size = buildTarget.data.currentBuildingSize;
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
            lineRenderer.SetPosition(1, new Vector3(buildTarget.transform.position.x + 0.5f * size, buildTarget.transform.position.y + 0.5f * size, -1));
            lineRenderer.SetPosition(2, new Vector3(transform.position.x, transform.position.y, -1));
            lineRenderer.SetPosition(3, new Vector3(buildTarget.transform.position.x + 0.5f * size, buildTarget.transform.position.y - 0.5f * size, -1));
            lineRenderer.SetPosition(4, new Vector3(transform.position.x, transform.position.y, -1));
            lineRenderer.SetPosition(5, new Vector3(buildTarget.transform.position.x - 0.5f * size, buildTarget.transform.position.y + 0.5f * size, -1));
            lineRenderer.SetPosition(6, new Vector3(transform.position.x, transform.position.y, -1));
            lineRenderer.SetPosition(7, new Vector3(buildTarget.transform.position.x - 0.5f * size, buildTarget.transform.position.y - 0.5f * size, -1));
        }
        else
            LineDelete();
    }
    public void LineDelete()
    {
        lineRenderer.enabled = false;
    }
}
