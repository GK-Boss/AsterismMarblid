using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    static public EnemySpawnerManager instance;

    [SerializeField] private float bossRateAddtion;

    private EnemySpawner latestSpawner;
    private List<EnemySpawner> allSpawners = new();
    private int riskLevel = 1;
    private float riskLevel_F = 1;
    float currentBossRate;

    private void Awake()
    {
        instance = this;
    }

    public void Register(EnemySpawner spawner)
    {
        allSpawners.Add(spawner);
        latestSpawner = spawner;
    }

    public void UnRegister(EnemySpawner spawner)
    {
        allSpawners.Remove(spawner);
    }

    public void AddRiskLevel(float value)
    {
        riskLevel_F += value * 0.25f;
        riskLevel = Mathf.CeilToInt(riskLevel_F);
    }

    public int GetRiskLevel() => riskLevel;
    public int GetSpawnerNum() => allSpawners.Count;
    public void Spawn()
    {
        int enemyNum = Mathf.CeilToInt(riskLevel * Random.Range(0f, 1f) * Random.Range(0f, 1f));

        bool isBoss = Random.Range(0, 100) < currentBossRate;
        currentBossRate += isBoss ? -currentBossRate : bossRateAddtion;

        if (isBoss)
        {
            //int index = Mathf.FloorToInt(Mathf.Log10(riskLevel)) - 1;
            //if (index < 0)
            //    index = 0;
            latestSpawner.SpawnBoss();

            enemyNum = Mathf.FloorToInt(enemyNum / 2.0f);
        }

        // ŽG‹›“G
        float sumWeight = 10 + allSpawners.Count;

        foreach (var spawner in allSpawners)
        {
            if (spawner == latestSpawner)
                spawner.Spawn(Mathf.CeilToInt(10 * enemyNum / sumWeight));
            else
                spawner.Spawn(Mathf.CeilToInt(enemyNum / sumWeight));
        }
    }
}
