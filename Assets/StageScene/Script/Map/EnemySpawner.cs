using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnInfo[] boss;
    [SerializeField] private SpawnInfo[] enemies;

    private void Awake()
    {
        EnemySpawnerManager.instance.Register(this);
    }
    public void SpawnBoss()
    {
        float sumAppearance = 0;
        foreach (var b in boss)
            sumAppearance += b.appearance;

        if (sumAppearance <= 0) return;

        float randomNum = Random.Range(0, sumAppearance);

        // 2. 正しい重み付け抽選（appearance を使う）
        foreach (var b in boss)
        {
            if (randomNum < b.appearance)
            {
                EnemyBase eb = PoolManager.instance.Get<EnemyBase>(b.enemy.gameObject);
                eb.transform.position = transform.position;

                break;
            }
            randomNum -= b.appearance;
        }

    }
    public void Spawn(int cost)
    {
        // 1. 出現率（重み）の合計を計算
        float sumAppearance = 0;
        foreach (var enemy in enemies)
            sumAppearance += enemy.appearance;

        // 安全対策：敵リストが空、または出現率がすべて0なら処理しない
        if (sumAppearance <= 0) return;

        // 無限ループ防止用のカウンター（万が一のための安全弁）
        int safetyCounter = 0;

        while (cost > 0 && safetyCounter < 1000)
        {
            safetyCounter++;

            SpawnInfo si = null;
            float randomNum = Random.Range(0, sumAppearance);

            // 2. 正しい重み付け抽選（appearance を使う）
            foreach (var enemy in enemies)
            {
                if (randomNum < enemy.appearance)
                {
                    si = enemy;
                    break;
                }
                randomNum -= enemy.appearance;
            }

            if (si == null) break;

            // 3. 残りコストで購入可能かチェック (<= に修正)
            if (si.enemy.cost <= cost)
            {
                EnemyBase eb = PoolManager.instance.Get<EnemyBase>(si.enemy.gameObject);
                eb.transform.position = transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));

                cost -= si.enemy.cost;
            }
            else
            {
                // 【重要】選ばれた敵のコストが高すぎて買えなかった場合
                // 現在の残りコストで買える敵が「まだリストにいるか」をチェックする
                bool canAffordAny = false;
                foreach (var enemy in enemies)
                {
                    if (enemy.enemy.cost <= cost)
                    {
                        canAffordAny = true;
                        break;
                    }
                }

                // もうどの敵も買えないなら、ループを終了する
                if (!canAffordAny)
                {
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class SpawnInfo
{
    [SerializeField] private string name;
    public EnemyBase enemy;
    public float appearance;
}
