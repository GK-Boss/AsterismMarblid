using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private CameraCtrl cc;
    private void Start()
    {
        menu.SetActive(false);
    }
    public void SwitchMenu()
    {
        if (!cc)
            cc = Camera.main.GetComponent<CameraCtrl>();

        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
            cc.stop = false;
        }
        else
        {
            menu.SetActive(true);
            cc.stop = true;
        }
    }
}
