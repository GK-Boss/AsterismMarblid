
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class ResourceOutputModule : BuildingModule
{
    [Header("建物基準の排出方向")]
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

        // 建物が占有しているすべてのセルを取得
        List<Vector3Int> occupiedCells =
            Utility.PositionAndSizeToAllPosInt(
                building.transform.position,
                building.data.currentBuildingSize
            );

        // 搬出可能な方向と、その方向の搬入先を保存
        List<Direction> availableDirections = new();

        List<List<ResourceInputModule>> availableInputs = new();

        foreach (var localDir in outputDirections)
        {
            Direction worldDir =
                Utility.LocalToWorld(localDir, building.GetDirection());

            Vector3Int offset =
                Utility.DirectionToVector(worldDir);

            // この方向に搬入可能な建物を探す
            List<ResourceInputModule> inputs = new();

            foreach (Vector3Int cell in occupiedCells)
            {
                Vector3Int targetPos = cell + offset;

                var next =
                    MapManager.instance.GetBuilding(targetPos);

                if (next == null || next == building) continue;

                if (next.buildState != BuildingBase.BuildState.Built)
                    continue;

                if (!next.TryGetComponent<ResourceInputModule>(out var input))
                    continue;

                Direction fromDir = Utility.Opposite(worldDir);

                if (!input.CanReceive(fromDir))
                    continue;

                // 同じ建物を重複登録しない
                if (!inputs.Contains(input))
                {
                    inputs.Add(input);
                }
            }

            // 搬出可能な建物がある方向だけ登録
            if (inputs.Count > 0 && !availableDirections.Contains(worldDir))
            {
                availableDirections.Add(worldDir);
                availableInputs.Add(inputs);
            }
        }

        // 搬出可能な方向がなければ終了
        if (availableDirections.Count <= 0) return;

        // 搬出方向をランダムに選択
        int directionIndex =
            Random.Range(0, availableDirections.Count);

        Direction selectedDirection =
            availableDirections[directionIndex];

        // 選択した方向にある搬入先からランダムに選択
        List<ResourceInputModule> selectedInputs =
            availableInputs[directionIndex];

        int inputIndex =
            Random.Range(0, selectedInputs.Count);

        ResourceInputModule selectedInput =
            selectedInputs[inputIndex];

        // 相手から見た搬入方向
        Direction fromDirection =
            Utility.Opposite(selectedDirection);

        res.fromDirection = fromDirection;

        selectedInput.Receive(res);

        inventory.RemoveAt(0);
    }
}
