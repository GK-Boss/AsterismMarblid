using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class MiningModule : BuildingModule
{
    private List<Resource> output = new();

    private float timer;

    private InventoryModule inventory;

    protected override void Awake()
    {
        base.Awake();

        inventory = GetComponent<InventoryModule>();
        Tile tile = MapManager.instance.GetTile(Vector3Int.RoundToInt(transform.position));
        Vein vein = MapManager.instance.GetVein(Vector3Int.RoundToInt(transform.position));
        if (tile && tile.resource)
        {
            output.Add(tile.resource);
        }
        if (vein && vein.resource)
        {
            output.Add(vein.resource);
        }
    }
    public void ReLoad()
    {
        output.Clear();
        Awake();
    }
    public override void OnTick(float dt)
    {
        if (output.Count == 0)
        {
            ReLoad();
            return;
        }
        if (!building || building.buildState != BuildingBase.BuildState.Built) return;
        if (!inventory.CanAccept()) return;

        timer += dt;

        while (timer >= 1f / data.defaultMiningSpeed)
        {
            timer -= 1f / data.defaultMiningSpeed;

            int randomNum = Random.Range(0, output.Count);

            ResourcePrefab res = PoolManager.instance.Get<ResourcePrefab>(output[randomNum].prefab.gameObject);
            res.ChangeVisivle(false);
            inventory.Add(res);
        }
    }
}