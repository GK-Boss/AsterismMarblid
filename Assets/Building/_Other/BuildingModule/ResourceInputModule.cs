using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class ResourceInputModule : BuildingModule
{
    [Header("右向き建物基準の搬入方向")]
    [SerializeField]
    private List<Direction> inputDirections = new();

    private InventoryModule inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<InventoryModule>();
    }

    // fromDir = 相手から来る方向（ワールド方向）
    public bool CanReceive(Direction fromDir)
    {
        if (!building) return false;

        if (building.buildState != BuildingBase.BuildState.Built)
            return false;

        if (!inventory.CanAccept())
            return false;

        Direction local = Utility.WorldToLocal(fromDir, building.GetDirection());

        return inputDirections.Contains(local);
    }

    public void Receive(ResourcePrefab res)
    {
        inventory.Add(res);
    }


    public override void OnTick(float dt)
    {

    }
}