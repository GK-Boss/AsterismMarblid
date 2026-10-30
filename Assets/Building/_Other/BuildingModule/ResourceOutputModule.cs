using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class ResourceOutputModule : BuildingModule
{
    [Header("Œš•¨Šî€‚Ì”ro•ûŒü")]
    [SerializeField]
    private List<Direction> outputDirections = new();

    public float interval = 0.2f;

    private float timer;

    private InventoryModule inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<InventoryModule>();
    }

    public override void OnTick(float dt)
    {
        if (!building) return;

        if (building.buildState != BuildingBase.BuildState.Built) return;
        timer += inventory.Count <= 0 ? -timer : dt;
        if (timer < interval) return;
        ResourcePrefab res = inventory.Peek(0);
        if (res == null) return;
        timer = 0;

        foreach (var localDir in outputDirections)
        {
            Direction worldDir = Utility.LocalToWorld(localDir, building.GetDirection());

            Vector3Int offset = Utility.DirectionToVector(worldDir);

            var next = MapManager.instance.GetBuilding(Vector3Int.RoundToInt(transform.position) + offset);

            if (next == null || next.buildState != BuildingBase.BuildState.Built) continue;
            if (!next.TryGetComponent<ResourceInputModule>(out var input)) continue;

            Direction fromDir = Utility.Opposite(worldDir); // ‘Šè‚©‚ç‚İ‚½”À“ü•ûŒü

            if (!input.CanReceive(fromDir)) continue;

            res.fromDirection = fromDir;
            input.Receive(res);
            inventory.RemoveAt(0);
            return;
        }
    }
}