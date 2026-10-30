using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD2 : MonoBehaviour
{
    static public HUD2 instance;

    [SerializeField] private Image modeBar;

    [Header("各モード")]
    [SerializeField] private IdleMode idleMode;
    [SerializeField] private BuildMode buildMode;
    [SerializeField] private DestroyMode destroyMode;
    [SerializeField] private RepairMode repairMode;
    [SerializeField] private CopyMode copyMode;
    [SerializeField] private BombMode bombMode;

    [Header("各ボタン")]
    [SerializeField] private Image idleModeButton;
    [SerializeField] private Image buildModeButton;
    [SerializeField] private Image destroyModeButton;
    [SerializeField] private Image repairModeButton;
    [SerializeField] private Image copyModeButton;
    [SerializeField] private Image bombModeButton;
    [Header("決定ボタン")]
    [SerializeField] private Image disideButton;

    [SerializeField] private BuildingCategories buildingCategories;

    [Header("フレーム")]
    [SerializeField] private Image frame;

    [Header("各パネル")]
    [SerializeField] private BuildingInfomationPanel buildingInfomationPanel;
    [SerializeField] private GameObject bombInfomationPanel;
    [SerializeField] private BuildingCostPanel costPanel;

    private BuildingData selectBuilding;
    private GameObject emphasisObj;
    private Mode mode;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.Log("HUD2*2");
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (MapManager.instance.GetCanDiside())
        {
            buildModeButton.gameObject.SetActive(false);
            disideButton.gameObject.SetActive(true);
        }
        else
        {
            buildModeButton.gameObject.SetActive(true);
            disideButton.gameObject.SetActive(false);
        }

        if (selectBuilding)
        {
            costPanel.gameObject.SetActive(true);
            costPanel.GenerateCostUI(selectBuilding);
            frame.enabled = true;
            frame.transform.position = emphasisObj.transform.position;
        }
        else
        {
            costPanel.gameObject.SetActive(false);
            frame.enabled = false;
        }
    }
    public void ChangeMode_Int(int _mode)
    {
        switch (_mode)
        {
            case 0:
                ChangeMode(Mode.Idle);
                break;
            case 1:
                ChangeMode(Mode.Build);
                break;
            case 2:
                ChangeMode(Mode.Destroy);
                break;
            case 3:
                ChangeMode(Mode.Repair);
                break;
            case 4:
                ChangeMode(Mode.Copy);
                break;
            case 5:
                ChangeMode(Mode.Bomb);
                break;
        }
    }
    public void ChangeMode(Mode _mode, bool absolute = false)
    {
        if (_mode == mode && !absolute)
            mode = Mode.Idle;
        else
            mode = _mode;

        ResetButtonColor();
        ResetMode();
        NoneSelectBuilding();
        ResetAllPanel();
        buildingCategories.NoneSelectCategory();

        switch (mode)
        {
            case Mode.Idle:
                idleMode.gameObject.SetActive(true);
                //idleModeButton.color = Color.yellow;
                modeBar.color = Color.white;
                break;
            case Mode.Build:
                buildMode.gameObject.SetActive(true);
                buildModeButton.color = Color.yellow;
                modeBar.color = Color.yellow;
                break;
            case Mode.Destroy:
                destroyMode.gameObject.SetActive(true);
                destroyModeButton.color = Color.red;
                modeBar.color = Color.red;
                break;
            case Mode.Repair:
                repairMode.gameObject.SetActive(true);
                repairModeButton.color = Color.green;
                modeBar.color = Color.green;
                break;
            case Mode.Copy:
                copyMode.gameObject.SetActive(true);
                copyModeButton.color = Color.cyan;
                modeBar.color = Color.cyan;
                break;
            case Mode.Bomb:
                bombMode.gameObject.SetActive(true);
                bombModeButton.color = Color.red;
                modeBar.color = Color.black;
                break;
        }
    }

    private void ResetButtonColor()
    {
        idleModeButton.color = Color.white;
        buildModeButton.color = Color.white;
        //disideButton.color = Color.white;
        destroyModeButton.color = Color.white;
        repairModeButton.color = Color.white;
        copyModeButton.color = Color.white;
        bombModeButton.color = Color.white;
    }
    private void ResetMode()
    {
        idleMode.gameObject.SetActive(false);
        buildMode.gameObject.SetActive(false);
        destroyMode.gameObject.SetActive(false);
        repairMode.gameObject.SetActive(false);
        copyMode.gameObject.SetActive(false);
        bombMode.gameObject.SetActive(false);
    }

    public void SelectBuilding(BuildingData building)
    {
        if (building != selectBuilding)
        {
            selectBuilding = building;
            BuildingSelectCtrl.instance.selectBuilding = selectBuilding;//
        }
        else
            NoneSelectBuilding();
    }
    public void NoneSelectBuilding()
    {
        selectBuilding = null;
        BuildingSelectCtrl.instance.selectBuilding = null;
        emphasisObj = null;
    }

    public void Emphasis(GameObject image)
    {
        emphasisObj = image;
    }

    #region パネル

    public void OpenBuildingInfomationPanel(BuildingBase building)
    {
        ResetAllPanel();

        buildingInfomationPanel.gameObject.SetActive(true);
        buildingInfomationPanel.SetInfomation(building);
    }

    public void ResetAllPanel()
    {
        buildingInfomationPanel.gameObject.SetActive(false);
    }
    #endregion

}

[System.Serializable]
public enum Mode
{
    Idle, Build, Destroy, Repair, Copy, Bomb
}

