using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerGenerator : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private int amount;
    [SerializeField] private int firstMaxDistance;

    private List<Vector3Int> generatedPos = new();
    public IEnumerator GenerateCoroutine(Vector3Int centerPos, List<Vector3Int> allPositions)
    {
        int counter = 0;
        while (amount > 0)
        {
            Vector3Int[] posArray = allPositions.ToArray();
            int randomNum = Random.Range(0, posArray.Length);
            Vector3Int selectedPos = posArray[randomNum];

            if (Vector3Int.Distance(selectedPos, centerPos) >= firstMaxDistance && MapManager.instance.GetCanBuild(selectedPos) && !generatedPos.Contains(selectedPos))
            {
                amount--;
                generatedPos.Add(selectedPos);
            }

            firstMaxDistance--;

            if (counter >= 1000)
            {
                counter = 0;
                yield return null;
            }
            else
                counter++;
        }
    }

    public void ChangeSpawner(Vector3Int posInt)
    {
        if (generatedPos.Contains(posInt))
            Instantiate(spawner, posInt, Quaternion.identity);
    }
}
