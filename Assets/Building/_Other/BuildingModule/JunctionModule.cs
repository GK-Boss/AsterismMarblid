using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class JunctionModule : BuildingModule
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    const float SPACING = 0.5f;

    private InventoryModule inventory;

    private readonly List<JunctionResource> resources = new();

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

            resources.Add(new JunctionResource
            {
                resource = peek,
                horizontal = peek.fromDirection == Direction.Right || peek.fromDirection == Direction.Left,
                progress = 0
            });

            peek.ChangeVisivle(false);
        }
        while (resources.Count > inventory.Count)
        {
            resources.RemoveAt(resources.Count - 1);
        }

        Vector3 middle = transform.position;
        Vector3 end = middle + (Vector3)Utility.DirectionToVector(building.GetDirection()) / 2.0f;

        // 進行
        float pastProgress_H = 1.5f;
        float pastProgress_V = 1.5f;
        for (int i = 0; i < resources.Count; i++)
        {
            JunctionResource jr = resources[i];

            if (jr.horizontal) // 水平
            {
                if (pastProgress_H == 1.5f)
                    jr.progress = Mathf.Min(jr.progress + dt * moveSpeed, 1);
                else
                    jr.progress = Mathf.Min(jr.progress + dt * moveSpeed, pastProgress_H - SPACING);

                pastProgress_H = Mathf.Min(pastProgress_H, jr.progress);

                // 搬出
                if (jr.progress >= 1)
                {
                    Vector3Int offset = Utility.DirectionToVector(Utility.Opposite(jr.resource.fromDirection));

                    var next = MapManager.instance.GetBuilding(Vector3Int.RoundToInt(transform.position) + offset);

                    if (next == null || next.buildState != BuildingBase.BuildState.Built) continue;
                    if (!next.TryGetComponent<ResourceInputModule>(out var input)) continue;

                    if (!input.CanReceive(jr.resource.fromDirection)) continue;

                    input.Receive(jr.resource);
                    inventory.RemoveAt(i);
                    resources.RemoveAt(i);
                }
            }
            else // 垂直
            {
                if (pastProgress_V == 1.5f)
                    jr.progress = Mathf.Min(jr.progress + dt * moveSpeed, 1);
                else
                    jr.progress = Mathf.Min(jr.progress + dt * moveSpeed, pastProgress_V);

                pastProgress_V = Mathf.Min(pastProgress_V, jr.progress);

                // 搬出
                if (jr.progress >= 1)
                {
                    Vector3Int offset = Utility.DirectionToVector(Utility.Opposite(jr.resource.fromDirection));

                    var next = MapManager.instance.GetBuilding(Vector3Int.RoundToInt(transform.position) + offset);

                    if (next == null || next.buildState != BuildingBase.BuildState.Built) continue;
                    if (!next.TryGetComponent<ResourceInputModule>(out var input)) continue;

                    if (!input.CanReceive(jr.resource.fromDirection)) continue;

                    input.Receive(jr.resource);
                    inventory.RemoveAt(i);
                    resources.RemoveAt(i);
                }
            }

        }

    }

    private class JunctionResource
    {
        public ResourcePrefab resource;
        public bool horizontal;
        public float progress;
    }
}
