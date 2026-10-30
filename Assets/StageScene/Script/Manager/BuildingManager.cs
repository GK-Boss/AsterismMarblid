using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    private List<IBuildingTick> ticks = new List<IBuildingTick>();

    void Awake()
    {
        instance = this;
    }

    public void Register(IBuildingTick t)
    {
        ticks.Add(t);
    }

    public void Unregister(IBuildingTick t)
    {
        ticks.Remove(t);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < ticks.Count; i++)
        {
            ticks[i].OnTick(dt);
        }
    }
}