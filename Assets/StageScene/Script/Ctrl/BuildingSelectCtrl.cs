using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectCtrl : MonoBehaviour
{
    static public BuildingSelectCtrl instance;

    [SerializeField] private Image frame;

    [HideInInspector] public BuildingData selectBuilding;

    private GameObject emphasisObj;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("BuildingSelectCtrlÇÃèdï°");
    }
}
