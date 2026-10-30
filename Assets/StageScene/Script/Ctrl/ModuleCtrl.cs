using UnityEngine;
using System.Collections.Generic;

public class ModuleCtrl : MonoBehaviour
{
    static public ModuleCtrl instance;

    public List<Module> modules = new();

    private void Awake()
    {
        instance = this;
    }
}
