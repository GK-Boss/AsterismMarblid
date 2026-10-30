using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretModule : BuildingModule
{
    [SerializeField] private LayerMask enemyLayer;

    private InventoryModule inventory;
    private GameObject target;
    private float intervalTime;

    private void Start()
    {
        inventory = GetComponent<InventoryModule>();
        InvokeRepeating(nameof(GetTarget), 0, 0.2f);
    }

    public void SetData(BuildingData _data)
    {
        data = _data;
    }
    public override void OnTick(float dt)
    {
        if (!target)
        {
            intervalTime = 0;
            return;
        }

        intervalTime += dt;
        if (intervalTime >= 1.0f / data.currentRate)
        {
            GameObject bullet = SelectBullet();
            if (!bullet) return;

            BulletBase bulletBase = PoolManager.instance.Get<BulletBase>(bullet);
            bulletBase.transform.position = transform.position;
            bulletBase.Init(target, data);
            intervalTime = 0;
        }
    }

    private void GetTarget()
    {
        if (target && target.activeInHierarchy && (target.transform.position - transform.position).magnitude <= data.currentRange)
        {
            return;
        }

        float minDistance = Mathf.Infinity;
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, data.currentRange, enemyLayer);

        target = null;
        for (int i = 0; i < results.Length; i++)
        {
            if (!results[i].gameObject.activeInHierarchy) continue;
            float distance = (results[i].transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                target = results[i].gameObject;
                minDistance = distance;
            }
        }
    }
    private GameObject SelectBullet()
    {
        List<GameObject> bulletList = new();
        foreach (var recipe in data.defaultBulletRecipes)
        {
            if (inventory.CanTake(recipe.resource))
            {
                bulletList.Add(recipe.bulletPrefab);
            }
        }

        if (bulletList.Count > 0)
            return bulletList[Random.Range(0, bulletList.Count)];
        else
            return null;
    }
}