using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinGenerator : MonoBehaviour
{
    [SerializeField] private PerlinNoiseVein[] perlinNoiseVeins;
    [SerializeField] private float noiseScale = 10f;

    private Vector2 perlinNoiseSeed;
    private Dictionary<Vector3Int, Vein> allVeins = new();

    private void Start()
    {
        // パーリンノイズのシード値をランダムに設定
        perlinNoiseSeed = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
    }

    public IEnumerator GenerateCoroutine(List<Vector3Int> area)
    {
        allVeins.Clear(); // 重複エラー防止
        yield return StartCoroutine(GenerateVein(area));
        yield return StartCoroutine(RegisterAllVein());
    }

    private IEnumerator GenerateVein(List<Vector3Int> area)
    {
        yield return null;
        foreach (Vector3Int posInt in area)
        {
            // その座標にある現在のタイルを取得
            Tile currentTile = MapManager.instance.GetTile(posInt);
            if (currentTile == null)
            {
                allVeins[posInt] = null;
                continue;
            }

            // 【修正ポイント】そのタイル（地面）の上に生成可能な鉱床のリストを抽出
            List<PerlinNoiseVein> validVeins = new List<PerlinNoiseVein>();
            int totalAppearance = 0;

            foreach (var pv in perlinNoiseVeins)
            {
                // underTilesの中に、現在のタイルが含まれているかチェック
                if (System.Array.Exists(pv.underTiles, t => t == currentTile))
                {
                    validVeins.Add(pv);
                    totalAppearance += pv.appearance;
                }
            }

            // 生成可能な鉱床が1つもない場合はnull
            if (validVeins.Count == 0 || totalAppearance == 0)
            {
                allVeins[posInt] = null;
                continue;
            }

            // パーリンノイズから 0.0 ～ 1.0 の値を取得
            float sampleX = (posInt.x + perlinNoiseSeed.x) / noiseScale;
            float sampleY = (posInt.y + perlinNoiseSeed.y) / noiseScale;
            float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

            // 抽出された候補の中から、ノイズ値を使って決定（しきい値判定）
            float currentWeight = 0f;
            Vein selectedVein = null;

            foreach (var pv in validVeins)
            {
                currentWeight += (float)pv.appearance / totalAppearance;
                if (noiseValue <= currentWeight)
                {
                    selectedVein = pv.vein;
                    break;
                }
            }

            // 辞書に登録（Addではなくインデクサ[]を使うことで重複上書きエラーを回避）
            allVeins[posInt] = selectedVein;
        }
    }

    private IEnumerator RegisterAllVein()
    {
        foreach (var tile in allVeins)
        {
            MapManager.instance.RegisterVein(tile.Key, tile.Value);
        }
        yield return null;
    }

    public void ChangeVein(Vector3Int posInt)
    {
        if (allVeins.ContainsKey(posInt) && allVeins[posInt] != null)
            MapManager.instance.ChangeVein(posInt, allVeins[posInt]);
    }
}


/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class VeinGenerator : MonoBehaviour
{
    [SerializeField] private PerlinNoiseVein[] perlinNoiseVeins;
    [SerializeField] private float noiseScale = 1;
    private Vector2 perlinNoiseSeed;
    private int sumAppearance;
    private Dictionary<Vector3Int, Vein> allVeins = new();
    private void Start()
    {
        perlinNoiseSeed = new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
        foreach (PerlinNoiseVein pt in perlinNoiseVeins)
            sumAppearance += pt.appearance;
    }
    public IEnumerator GenerateCoroutine(List<Vector3Int> area)
    {
        yield return StartCoroutine(GenerateVein(area));
        yield return StartCoroutine(RegisterAllVein());
    }
    private IEnumerator GenerateVein(List<Vector3Int> area)
    {
        yield return null;
        foreach (Vector3Int posInt in area)
        {
            PerlinNoiseVein selectedVein = SelectVein(posInt);

            if (MapManager.instance.GetTile(posInt) != null && selectedVein.underTiles.Contains(MapManager.instance.GetTile(posInt)))
            {
                //Debug.Log(selectedVein.vein);
                allVeins.Add(posInt, selectedVein.vein);
            }
            else
            {
                allVeins.Add(posInt, null);
            }
        }
    }

    private PerlinNoiseVein SelectVein(Vector3Int posInt)
    {
        float perlinNoiseValue = Mathf.PerlinNoise((posInt.x + perlinNoiseSeed.x) / noiseScale, (posInt.y + perlinNoiseSeed.y) / noiseScale) * sumAppearance;
        if (perlinNoiseValue > sumAppearance)
            perlinNoiseValue = sumAppearance;

        foreach (PerlinNoiseVein pt in perlinNoiseVeins)
        {
            perlinNoiseValue -= pt.appearance;
            if (perlinNoiseValue <= 0)
                return pt;
        }
        return null;
    }

    private IEnumerator RegisterAllVein()
    {
        foreach (var tile in allVeins)
        {
            MapManager.instance.RegisterVein(tile.Key, tile.Value);
        }
        yield return null;
    }

    public void ChangeVein(Vector3Int posInt)
    {
        if (allVeins[posInt] != null)
            MapManager.instance.ChangeVein(posInt, allVeins[posInt]);
    }
}
*/
[System.Serializable]
public class PerlinNoiseVein
{
    public Vein vein;
    [Header("下になりうるタイルたち")]
    public Tile[] underTiles;
    [Header("タイルの出現率")]
    public int appearance;
}

