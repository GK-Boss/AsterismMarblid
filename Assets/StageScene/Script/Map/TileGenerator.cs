using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("最小範囲")]
    [SerializeField] private int minWidth;
    [SerializeField] private int minHeight;

    [Header("最大範囲")]
    [SerializeField] private int maxWidth;
    [SerializeField] private int maxHeight;

    [Header("高いほど基準点がずれる")]
    [SerializeField] private int centerNoise;

    [Header("高いほど地形がガタガタになる")]
    [SerializeField] private int noise;

    [Header("高いほど地形のガタガタを丸くする")]
    [SerializeField] private int correction;

    [Header("低いほど地形が細かくなる")]
    [SerializeField] private float noiseScale = 1;

    [Header("出現しうるタイル")]
    [SerializeField] private PerlinNoiseTile[] perlinNoiseTiles;

    private Dictionary<Vector3Int, Tile> allTiles = new();
    private List<Vector3Int> outerPositions = new();
    private Vector2 perlinNoiseSeed;
    int sumAppearance = 0;

    private void Start()
    {
        perlinNoiseSeed = new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
        foreach (PerlinNoiseTile pt in perlinNoiseTiles)
            sumAppearance += pt.appearance;
    }

    public IEnumerator GenerateTile()
    {
        yield return StartCoroutine(GenerateGround());
        yield return StartCoroutine(GenerateWall());

        yield return VerifyValidTile();
        yield return RegisterAllTile();
    }

    private IEnumerator GenerateGround()
    {
        Vector3Int centerPosInt = Vector3Int.RoundToInt(transform.position);

        int width = Random.Range(minWidth, maxWidth);
        int height = Random.Range(minHeight, maxHeight);

        int halfWidth = width % 2 == 0 ? width / 2 : width / 2 + 1;
        int halfHeight = height % 2 == 0 ? height / 2 : height / 2 + 1;

        int randomWidthNoise = Random.Range(-centerNoise, centerNoise);
        int rightWidth = halfWidth + randomWidthNoise;
        int leftWidth = halfWidth - randomWidthNoise;

        int randomHeightNoise = Random.Range(-centerNoise, centerNoise);
        int upHeight = halfHeight + randomHeightNoise;
        int downHeight = halfHeight - randomHeightNoise;

        int pastNoise = 0;

        yield return null;
        #region Generate
        //第1象限
        // 右→上noise
        for (int x = 0; x < rightWidth; x++)
        {
            int randomNoise = Random.Range(0, noise);
            if (x > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)x / rightWidth));
            pastNoise = randomNoise;

            for (int y = 0; y < upHeight + randomNoise; y++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(x, y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (y >= upHeight - 1 || x == rightWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        // 上→右noise
        pastNoise = 0;
        for (int y = 0; y < upHeight; y++)
        {
            int randomNoise = Random.Range(0, noise);
            if (y > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)y / upHeight));
            pastNoise = randomNoise;

            for (int x = 0; x < rightWidth + randomNoise; x++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(x, y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (x >= rightWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        yield return null;

        //第2象限
        // 左→上noise
        pastNoise = 0;
        for (int x = 0; x < leftWidth; x++)
        {
            int randomNoise = Random.Range(0, noise);
            if (x > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)x / leftWidth));
            pastNoise = randomNoise;

            for (int y = 0; y < upHeight + randomNoise; y++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(-x, y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (y >= upHeight - 1 || x == leftWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        // 上→左noise
        pastNoise = 0;
        for (int y = 0; y < upHeight; y++)
        {
            int randomNoise = Random.Range(0, noise);
            if (y > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)y / upHeight));
            pastNoise = randomNoise;

            for (int x = 0; x < leftWidth + randomNoise; x++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(-x, y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (x >= leftWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        yield return null;

        //第3象限
        // 左→下noise
        pastNoise = 0;
        for (int x = 0; x < leftWidth; x++)
        {
            int randomNoise = Random.Range(0, noise);
            if (x > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)x / leftWidth));
            pastNoise = randomNoise;

            for (int y = 0; y < downHeight + randomNoise; y++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(-x, -y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (y >= downHeight - 1 || x == leftWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        // 下→左noise
        pastNoise = 0;
        for (int y = 0; y < downHeight; y++)
        {
            int randomNoise = Random.Range(0, noise);
            if (y > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)y / downHeight));
            pastNoise = randomNoise;

            for (int x = 0; x < leftWidth + randomNoise; x++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(-x, -y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (x >= leftWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        yield return null;

        //第4象限
        // 右→下noise
        pastNoise = 0;
        for (int x = 0; x < rightWidth; x++)
        {
            int randomNoise = Random.Range(0, noise);
            if (x > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)x / rightWidth));
            pastNoise = randomNoise;

            for (int y = 0; y < downHeight + randomNoise; y++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(x, -y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (y >= downHeight - 1 || x == rightWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        // 下→右noise
        pastNoise = 0;
        for (int y = 0; y < downHeight; y++)
        {
            int randomNoise = Random.Range(0, noise);
            if (y > 0)
                randomNoise = pastNoise + ((randomNoise - pastNoise) / (correction + 1));
            randomNoise = Mathf.RoundToInt(randomNoise * (1.0f - (float)y / downHeight));
            pastNoise = randomNoise;

            for (int x = 0; x < rightWidth + randomNoise; x++)
            {
                Vector3Int currentPosInt = centerPosInt + new Vector3Int(x, -y);
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                {
                    allTiles.Add(currentPosInt, SelectTile(currentPosInt));

                    if (x >= rightWidth - 1)
                    {
                        outerPositions.Add(currentPosInt);
                    }
                }
            }
        }
        #endregion
    }
    private Tile SelectTile(Vector3Int posInt)
    {
        float perlinNoiseValue = Mathf.PerlinNoise((posInt.x + perlinNoiseSeed.x) / noiseScale, (posInt.y + perlinNoiseSeed.y) / noiseScale) * sumAppearance;
        if (perlinNoiseValue >= sumAppearance)
            perlinNoiseValue = sumAppearance;
        foreach (PerlinNoiseTile pt in perlinNoiseTiles)
        {
            perlinNoiseValue -= pt.appearance;
            if (perlinNoiseValue <= 0)
                return pt.tile;
        }
        Debug.Log("SelectTile");
        return TileDictionary.instance.GetTile("Tile_Stone");
    }

    private IEnumerator GenerateWall()
    {
        Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };

        int waitCount = 0;
        foreach (Vector3Int outerPosInt in outerPositions)
        {
            foreach (Vector3Int dir in directions)
            {
                Vector3Int currentPosInt = outerPosInt + dir;
                if (!MapManager.instance.HasTile(currentPosInt) && !allTiles.ContainsKey(currentPosInt))
                    allTiles.Add(currentPosInt, TileDictionary.instance.GetTile("Tile_Wall"));
            }
            waitCount++;
            if (waitCount % 100 == 0)
                yield return null;
        }
    }

    private IEnumerator VerifyValidTile()
    {
        Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
        Queue<Vector3Int> willOpenPos = new();
        willOpenPos.Enqueue(Vector3Int.RoundToInt(transform.position));
        List<Vector3Int> validPosInt = new();

        while (willOpenPos.Count > 0)
        {
            Vector3Int centerPosInt = willOpenPos.Dequeue();

            foreach (Vector3Int dir in directions)
            {
                Vector3Int currentPosInt = centerPosInt + dir;
                if (allTiles.ContainsKey(currentPosInt) && !validPosInt.Contains(currentPosInt) && allTiles[currentPosInt])
                {
                    willOpenPos.Enqueue(currentPosInt);
                    validPosInt.Add(currentPosInt);
                }
            }
        }

        List<Vector3Int> willRemovePosInt = new();
        foreach (var tile in allTiles)
        {
            if (!validPosInt.Contains(tile.Key))
            {
                willRemovePosInt.Add(tile.Key);
            }
        }

        foreach (Vector3Int posInt in willRemovePosInt)
        {
            allTiles.Remove(posInt);
        }

        yield return null;
    }

    private IEnumerator RegisterAllTile()
    {
        Biome biome = transform.parent.GetComponent<Biome>();
        foreach (var tile in allTiles)
        {
            MapManager.instance.RegisterTile(tile.Key, tile.Value);
            biome.RegisterPosition(tile.Key);
        }
        yield return null;
    }

    public void ChangeTile(Vector3Int posInt)
    {
        if (allTiles.ContainsKey(posInt))
            MapManager.instance.ChangeTile(posInt, allTiles[posInt]);
        else
            Debug.Log("ChangeTile" + posInt.x + " , " + posInt.y);
    }
}

[System.Serializable]
public class PerlinNoiseTile
{
    public Tile tile;
    [Header("タイルの出現率")]
    public int appearance;
}
