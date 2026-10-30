using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAreaLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void LineDraw(Vector3 startPosInt, Vector3 endPosInt)
    {
        lineRenderer.enabled = true;
        startPosInt.z = -1;
        endPosInt.z = -1;
        lineRenderer.SetPosition(0, startPosInt);
        lineRenderer.SetPosition(1, new(endPosInt.x, startPosInt.y));
        lineRenderer.SetPosition(2, endPosInt);
        lineRenderer.SetPosition(3, new(startPosInt.x, endPosInt.y));
        lineRenderer.SetPosition(4, startPosInt);
    }
    public void LineDelete()
    {
        lineRenderer.enabled = false;
    }
}
