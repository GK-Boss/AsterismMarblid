using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class RouterModule : BuildingModule
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    const float SPACING = 0.5f;

    private InventoryModule inventory;

    private readonly List<RouterResource> resources = new();

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<InventoryModule>();
    }
    public override void OnTick(float dt)
    {
        if (!building || building.buildState != BuildingBase.BuildState.Built) return;

        // ç∑ÇÃèCê≥
        while (resources.Count < inventory.Count)
        {
            ResourcePrefab peek = inventory.Peek(resources.Count);

            if (peek == null)
                break;

            resources.Add(new RouterResource
            {
                resource = peek,
                progress = 0
            });

            peek.ChangeVisivle(false);
        }
        while (resources.Count > inventory.Count)
        {
            resources.RemoveAt(resources.Count - 1);
        }

        // êiçs
        float pastProgress = 1.5f;
        for (int i = 0; i < resources.Count; i++)
        {
            RouterResource cr = resources[i];

            if (i == 0)
                cr.progress = Mathf.Min(cr.progress + dt * moveSpeed, 1);
            else
                cr.progress = Mathf.Min(cr.progress + dt * moveSpeed, pastProgress - SPACING);

            pastProgress = Mathf.Min(pastProgress, cr.progress);

            // î¿èo
            if (cr.progress >= 1)
            {
                List<Direction> fromDirections = new();
                Dictionary<Direction, ResourceInputModule> directionToRIM = new();


                for (int j = 0; j < 4; j++)
                {
                    Direction currentDir = Direction.Right;

                    switch (j)
                    {
                        case 0:
                            currentDir = Direction.Right;
                            break;
                        case 1:
                            currentDir = Direction.Left;
                            break;
                        case 2:
                            currentDir = Direction.Up;
                            break;
                        case 3:
                            currentDir = Direction.Down;
                            break;
                    }

                    Vector3Int offset = Utility.DirectionToVector(currentDir);

                    var next = MapManager.instance.GetBuilding(Vector3Int.RoundToInt(transform.position) + offset);

                    if (next == null || next.buildState != BuildingBase.BuildState.Built) continue;
                    if (!next.TryGetComponent<ResourceInputModule>(out var input)) continue;

                    Direction fromDir = Utility.Opposite(currentDir); // ëäéËÇ©ÇÁÇ›ÇΩî¿ì¸ï˚å¸

                    if (!input.CanReceive(fromDir)) continue;

                    fromDirections.Add(fromDir);
                    directionToRIM.Add(fromDir, input);
                }

                if (fromDirections.Count <= 0) return;
                int randomIndex = Random.Range(0, directionToRIM.Count);

                cr.resource.fromDirection = fromDirections[randomIndex];
                directionToRIM[fromDirections[randomIndex]].Receive(cr.resource);
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

        //resources.Clear();
    }

    private class RouterResource
    {
        public ResourcePrefab resource;
        public float progress;
    }
}
