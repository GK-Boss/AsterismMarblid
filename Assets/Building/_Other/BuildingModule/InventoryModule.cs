using System.Collections.Generic;
using UnityEngine;

public class InventoryModule : BuildingModule
{
    private List<ResourcePrefab> resources = new();

    public List<ResourcePrefab> GetList() => resources; 
    public bool CanAccept()
    {
        return resources.Count < data.currentCapasity;
    }

    public void Add(ResourcePrefab res)
    {
        if (!CanAccept()) return;

        resources.Add(res);
    }

    public bool CanTake(Resource takeRes)
    {
        if (resources.Count <= 0)
            return false;

        foreach (var res in resources)
        {
            if (res.resource == takeRes)
            {
                resources.Remove(res);
                return true;
            }
        }

        return false;
    }
    public bool TryTake(out ResourcePrefab res)
    {
        if (resources.Count <= 0)
        {
            res = null;
            return false;
        }

        res = resources[0];
        resources.RemoveAt(0);

        return true;
    }

    public ResourcePrefab Peek(int index)
    {
        if (index < 0 || index >= resources.Count)
            return null;

        return resources[index];
    }
    public void RemoveAt(int index)
    {
        resources.RemoveAt(index);
    }

    public override void OnTick(float dt)
    {

    }

    public int Count => resources.Count;
}