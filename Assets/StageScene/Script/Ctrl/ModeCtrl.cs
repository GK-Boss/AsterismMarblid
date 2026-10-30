using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModeCtrl : MonoBehaviour
{
    static public ModeCtrl instance;

    //[SerializeField] private Image modeBar;

    //[Header("各モード")]
    //[SerializeField] private IdleMode idleMode;
    //[SerializeField] private BuildMode buildMode;
    //[SerializeField] private DestroyMode destroyMode;
    //[SerializeField] private RepairMode repairMode;
    //[SerializeField] private CopyMode copyMode;
    //[SerializeField] private BombMode bombMode;

    //[Header("各ボタン")]
    //[SerializeField] private Image idleModeButton;
    //[SerializeField] private Image buildModeButton;
    //[SerializeField] private Image destroyModeButton;
    //[SerializeField] private Image repairModeButton;
    //[SerializeField] private Image copyModeButton;
    //[SerializeField] private Image bombModeButton;
    //[Header("決定ボタン")]
    //[SerializeField] private Image disideButton;

    //private Mode mode;
    //private int buildNum;
    //private int destroyNum;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.Log("ModeCtrl*2");
            Destroy(gameObject);
        }
    }
    //private void Start()
    //{
    //    disideButton.color = Color.yellow;

    //    ChangeMode(mode);
    //}
    //public Mode GetCurrentMode()
    //{
    //    return mode;
    //}
    //public void SetBuildNum(int num)
    //{
    //    buildNum = num;
    //    ChangeMark(buildNum == 0 && destroyNum == 0);
    //}
    //public void SetDestroyNum(int num)
    //{
    //    destroyNum = num;
    //    ChangeMark(buildNum == 0 && destroyNum == 0);
    //}
    //private void ChangeMark(bool btdf)
    //{
    //    if (btdf)
    //    {
    //        buildModeButton.gameObject.SetActive(true);
    //        disideButton.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        buildModeButton.gameObject.SetActive(false);
    //        disideButton.gameObject.SetActive(true);
    //    }
    //}
    //public void ChangeMode(Mode _mode)
    //{
    //    if (_mode == mode)
    //        mode = Mode.Idle;
    //    else
    //        mode = _mode;

    //    ResetButtonColor();
    //    ResetMode();

    //    switch (mode)
    //    {
    //        case Mode.Idle:
    //            idleMode.gameObject.SetActive(true);
    //            //idleModeButton.color = Color.yellow;
    //            modeBar.color = Color.white;
    //            break;
    //        case Mode.Build:
    //            buildMode.gameObject.SetActive(true);
    //            buildModeButton.color = Color.yellow;
    //            //disideButton.color = Color.yellow;
    //            modeBar.color = Color.yellow;
    //            break;
    //        case Mode.Destroy:
    //            destroyMode.gameObject.SetActive(true);
    //            destroyModeButton.color = Color.red;
    //            modeBar.color = Color.red;
    //            break;
    //        case Mode.Repair:
    //            repairMode.gameObject.SetActive(true);
    //            repairModeButton.color = Color.green;
    //            modeBar.color = Color.green;
    //            break;
    //        case Mode.Copy:
    //            copyMode.gameObject.SetActive(true);
    //            copyModeButton.color = Color.cyan;
    //            modeBar.color = Color.cyan;
    //            break;
    //        case Mode.Bomb:
    //            bombMode.gameObject.SetActive(true);
    //            bombModeButton.color = Color.red;
    //            modeBar.color = Color.black;
    //            break;
    //    }
    //}

    //private void ResetButtonColor()
    //{
    //    idleModeButton.color = Color.white;
    //    buildModeButton.color = Color.white;
    //    //disideButton.color = Color.white;
    //    destroyModeButton.color = Color.white;
    //    repairModeButton.color= Color.white;
    //    copyModeButton.color =Color.white;
    //    bombModeButton.color = Color.white;
    //}
    //private void ResetMode()
    //{
    //    idleMode.gameObject.SetActive(false);
    //    buildMode.gameObject.SetActive(false);
    //    destroyMode.gameObject.SetActive(false);
    //    repairMode.gameObject.SetActive(false);
    //    copyMode.gameObject.SetActive(false);
    //    bombMode.gameObject.SetActive(false);
    //}
    //public enum Mode
    //{
    //    Idle, Build, Destroy, Repair, Copy, Bomb
    //}
}
