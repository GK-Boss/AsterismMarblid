using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static BuildingBase;

public class IdleMode : MonoBehaviour
{
    [SerializeField] private HUD2 hud2;
    void Update()
    {
        Idle();
    }
    private void Idle()
    {
        if (CheckUI()) return;

        Vector3Int mousePosInt = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePosInt.z = 0;
        BuildingBase overlapBuilding = MapManager.instance.GetBuilding(mousePosInt);

        if (!Input.GetKey(KeyCode.Mouse0))return;

        if (!overlapBuilding)
        {
            hud2.ResetAllPanel();
            return;
        }
        switch (overlapBuilding.buildState)
        {
            case BuildState.Ghost://削除
                overlapBuilding.Delete();
                break;
            case BuildState.Build://プログレスターゲット
                //Player.instance.progress.ChangeTarget(overlapBuilding);
                break;
            case BuildState.Built://詳細
                hud2.OpenBuildingInfomationPanel(overlapBuilding);
                break;
            case BuildState.WillDestroy://解除
                overlapBuilding.ChangeBuildState(BuildState.Built);
                break;
            case BuildState.Destroy://プログレスターゲット
                //Player.instance.progress.ChangeTarget(overlapBuilding);
                break;
        }
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
