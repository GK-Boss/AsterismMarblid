using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class ConveyorModule : BuildingModule
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    const float SPACING = 0.5f;

    private InventoryModule inventory;

    private readonly List<ConveyorResource> resources = new();

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<InventoryModule>();
    }
    public override void OnTick(float dt)
    {
        if (!building || building.buildState != BuildingBase.BuildState.Built) return;

        // 差の修正
        while (resources.Count < inventory.Count)
        {
            ResourcePrefab peek = inventory.Peek(resources.Count);

            if (peek == null)
                break;

            resources.Add(new ConveyorResource
            {
                resource = peek,
                progress = 0
            });

            peek.ChangeVisivle(true);
        }
        while (resources.Count > inventory.Count)
        {
            resources.RemoveAt(resources.Count - 1);
        }

        Vector3 middle = transform.position;
        Vector3 end = middle + (Vector3)Utility.DirectionToVector(building.GetDirection()) / 2.0f;

        // 進行
        float pastProgress = 1.5f;
        for (int i = 0; i < resources.Count; i++)
        {
            ConveyorResource cr = resources[i];

            if (i == 0)
                cr.progress = Mathf.Min(cr.progress + dt * moveSpeed, 1);
            else
                cr.progress = Mathf.Min(cr.progress + dt * moveSpeed, pastProgress - SPACING);

            // 移動
            Vector3 start = middle + (Vector3)Utility.DirectionToVector(cr.resource.fromDirection) / 2.0f;

            if (cr.progress < 0.5f)
                cr.resource.transform.position = Vector3.Lerp(start, middle, cr.progress * 2);
            else
                cr.resource.transform.position = Vector3.Lerp(middle, end, (cr.progress - 0.5f) * 2);

            pastProgress = Mathf.Min(pastProgress, cr.progress);

            // 搬出
            if (cr.progress >= 1)
            {
                Direction worldDir = building.GetDirection();

                Vector3Int offset = Utility.DirectionToVector(worldDir);

                var next = MapManager.instance.GetBuilding(Vector3Int.RoundToInt(transform.position) + offset);

                if (next == null || next.buildState != BuildingBase.BuildState.Built) continue;
                if (!next.TryGetComponent<ResourceInputModule>(out var input)) continue;

                Direction fromDir = Utility.Opposite(worldDir); // 相手からみた搬入方向

                if (!input.CanReceive(fromDir)) continue;

                cr.resource.fromDirection = fromDir;
                input.Receive(cr.resource);
                inventory.RemoveAt(i);
                resources.RemoveAt(i);
            }

        }
    }

    void OnDestroy()
    {
        foreach (var v in resources)
        {
            if (v.resource != null && v.resource.gameObject.activeInHierarchy)
                PoolManager.instance.Release(v.resource);
        }
    }

    private class ConveyorResource
    {
        public ResourcePrefab resource;
        public float progress;
    }
}