using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Biome : MonoBehaviour
{
    [Header("èoåªó¶(çÇÇ¢Ç∆èoÇ‚Ç∑Ç¢)")]
    public int appearanceRate = 1;

    [SerializeField] private TileGenerator tileGenerator;
    [SerializeField] private VeinGenerator veinGenerator;
    [SerializeField] private SpawnerGenerator spawnerGenerator;

    private Vector3Int centerPosInt;
    List<Vector3Int> allPositions = new();
    private void Start()
    {
        centerPosInt = Vector3Int.RoundToInt(transform.position);
        StartCoroutine(GenerateCoroutine());
    }
    private IEnumerator GenerateCoroutine()
    {
        yield return StartCoroutine(tileGenerator.GenerateTile());
        yield return StartCoroutine(veinGenerator.GenerateCoroutine(allPositions));
        yield return StartCoroutine(spawnerGenerator.GenerateCoroutine(centerPosInt, allPositions));

        int openedCount = 0;
        int i = 0;
        while (allPositions.Count > openedCount)
        {
            List<Vector3Int> changePosList = new();
            for (int x = -i; x <= i; ++x)
            {
                int y = i - Mathf.Abs(x);

                if (!changePosList.Contains(new(x, y)))
                    changePosList.Add(new(x, y));
                if (!changePosList.Contains(new(x, -y)))
                    changePosList.Add(new(x, -y));
            }

            foreach(Vector3Int posInt in changePosList)
            {
                Vector3Int currentPosInt = posInt + centerPosInt;
                if (allPositions.Contains(currentPosInt))
                {
                    tileGenerator.ChangeTile(currentPosInt);
                    veinGenerator.ChangeVein(currentPosInt);
                    spawnerGenerator.ChangeSpawner(currentPosInt);
                    openedCount++;
                }
            }
            i++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RegisterPosition(Vector3Int posInt)
    {
        allPositions.Add(posInt);
    }
}
