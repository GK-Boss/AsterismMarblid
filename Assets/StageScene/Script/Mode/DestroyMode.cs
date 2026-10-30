using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static BuildingBase;

public class DestroyMode : MonoBehaviour
{
    [SerializeField] private DestroyAreaLine destroyAreaLine;
    private Vector3Int startMousePosInt;
    private bool stopConider;
    private List<Vector3Int> destroyArea = new();
    private List<BuildingBase> willDestroy = new();
    private void Update()
    {
        MapManager.instance.SetDestroyNum(willDestroy.Count);

        Destroy();
    }
    private void Destroy()
    {
        Vector3Int mousePosInt = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePosInt.z = 0;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CheckUI())
            {
                stopConider = true;
                return;
            }
            BuildingBase overlapBuilding = MapManager.instance.GetBuilding(mousePosInt);
            startMousePosInt = mousePosInt;
            destroyArea.Clear();

            if (!overlapBuilding) return;

            switch (overlapBuilding.buildState)
            {
                case BuildState.Ghost://Ç»Çµ
                    break;
                case BuildState.Build://Ç»Çµ
                    break;
                case BuildState.Built://Ç»Çµ
                    break;
                case BuildState.WillDestroy://âèú
                    overlapBuilding.ChangeBuildState(BuildState.Built);
                    willDestroy.Remove(overlapBuilding);
                    stopConider = true;
                    break;
                case BuildState.Destroy://Ç»Çµ
                    break;
            }
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (stopConider) return;
            Vector3Int vec = mousePosInt - startMousePosInt;
            int signX = (int)Mathf.Sign(vec.x);
            int signY = (int)Mathf.Sign(vec.y);
            destroyAreaLine.LineDraw(new(startMousePosInt.x - 0.5f * signX, startMousePosInt.y - 0.5f * signY), new(mousePosInt.x + 0.5f * signX, mousePosInt.y + 0.5f * signY));
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (stopConider) return;
            int minX = Mathf.Min(startMousePosInt.x, mousePosInt.x);
            int maxX = Mathf.Max(startMousePosInt.x, mousePosInt.x);
            int minY = Mathf.Min(startMousePosInt.y, mousePosInt.y);
            int maxY = Mathf.Max(startMousePosInt.y, mousePosInt.y);

            Vector3Int posInt = Vector3Int.zero;
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    posInt.x = x;
                    posInt.y = y;
                    if (destroyArea.Contains(posInt)) continue;
                    BuildingBase overlapBuilding = MapManager.instance.GetBuilding(posInt);
                    if (overlapBuilding && !overlapBuilding.data.isStatic)
                    {
                        switch (overlapBuilding.buildState)
                        {
                            case BuildState.Ghost://çÌèú
                                overlapBuilding.Delete();
                                break;
                            case BuildState.Build://îjâÛ
                                overlapBuilding.ChangeBuildState(BuildState.WillDestroy);
                                willDestroy.Add(overlapBuilding);
                                break;
                            case BuildState.Built://îjâÛ
                                overlapBuilding.ChangeBuildState(BuildState.WillDestroy);
                                willDestroy.Add(overlapBuilding);
                                break;
                            case BuildState.WillDestroy://Ç»Çµ
                                break;
                            case BuildState.Destroy://Ç»Çµ
                                break;
                        }
                    }
                    destroyArea.Add(posInt);
                }
            }
            destroyAreaLine.LineDelete();
            destroyArea.Clear();
        }
        else
            stopConider = false;
    }
    public void Diside()
    {
        foreach (BuildingBase bb in willDestroy)
        {
            if (bb)
                bb.ChangeBuildState(BuildState.Destroy);
        }
        willDestroy.Clear();

        MapManager.instance.SetDestroyNum(0);
    }
    private bool CheckUI()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return true;

        List<RaycastResult> raycastResults = new();
        if (!EventSystem.current) return false;

        PointerEventData pointerEventData = new(EventSystem.current) { position = Input.mousePosition };
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count == 0) return false;

        return raycastResults[0].gameObject.layer == 5;
    }

}
