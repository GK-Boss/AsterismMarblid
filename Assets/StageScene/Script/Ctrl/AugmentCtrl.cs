using UnityEngine;
using System.Collections.Generic;

public class AugmentCtrl : MonoBehaviour
{
    static public AugmentCtrl instance;

    public List<Expansion> augments;

    private void Awake()
    {
        instance = this;
    }
}
