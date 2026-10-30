using UnityEngine;

[RequireComponent(typeof(BuildingBase))]
public abstract class BuildingModule : MonoBehaviour, IBuildingTick
{
    protected BuildingData data;
    protected BuildingBase building;

    public void SetBuildingData(BuildingData _data)
    {
        data = _data;
    }
    protected virtual void Awake()
    {
        building = GetComponent<BuildingBase>();
    }

    protected virtual void OnEnable()
    {
        if (BuildingManager.instance != null)
            BuildingManager.instance.Register(this);
    }

    protected virtual void OnDisable()
    {
        if (BuildingManager.instance != null)
            BuildingManager.instance.Unregister(this);
    }

    public abstract void OnTick(float dt);
}