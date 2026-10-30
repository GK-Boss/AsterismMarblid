
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class CoreModule : BuildingModule
{
    private InventoryModule inventory;

    private void Start()
    {
        inventory = GetComponent<InventoryModule>();
    }

    public override void OnTick(float dt)
    {
        while (inventory.TryTake(out ResourcePrefab res))
        {
            ResourceManager.instance.Add(res.resource);

            PoolManager.instance.Release(res);
        }
    }
}
