using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryModule))]
public class GenerateModule : BuildingModule
{
    [SerializeField] List<Resource> output = new();

    [Header("X / s")]
    [SerializeField] private float speed = 1f;

    private float timer;

    private InventoryModule inventory;

    protected override void Awake()
    {
        base.Awake();

        inventory = GetComponent<InventoryModule>();
    }

    public override void OnTick(float dt)
    {
        if (output.Count == 0) return;
        if (!building || building.buildState != BuildingBase.BuildState.Built) return;
        if (!inventory.CanAccept()) return;

        timer += dt;

        if (timer >= 1f / speed)
        {
            timer = 0f;

            int randomNum = Random.Range(0, output.Count);

            ResourcePrefab res = output[randomNum].prefab;
            inventory.Add(res);
        }
    }
}