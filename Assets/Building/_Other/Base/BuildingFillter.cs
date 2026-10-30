using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFillter : MonoBehaviour
{
    [SerializeField] private Sprite ghostFillter;
    [SerializeField] private Sprite buildFillter;
    [SerializeField] private Sprite willDestroyFillter;
    [SerializeField] private Sprite destroyFillter;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeFillter(BuildingBase.BuildState buildState)
    {
        switch (buildState)
        {
            case BuildingBase.BuildState.Ghost:
                sr.sprite = ghostFillter;
                break;
            case BuildingBase.BuildState.Build:
                sr.sprite = buildFillter;
                break;
            case BuildingBase.BuildState.Built:
                sr.sprite = null;
                break;
            case BuildingBase.BuildState.WillDestroy:
                sr.sprite = willDestroyFillter;
                break;
            case BuildingBase.BuildState.Destroy:
                sr.sprite = destroyFillter;
                break;
            default:
                break;
        }
    }
}
