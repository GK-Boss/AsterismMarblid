using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    public void SwitchMenu()
    {
        if (Menu.Menu.instance.isOpen)
            Menu.Menu.instance.CloseMenu();
        else
            Menu.Menu.instance.OpenMenu();
    }
}
