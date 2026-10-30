using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BiomeGenerator : MonoBehaviour
{
    [SerializeField] private Biome startBiome;
    [SerializeField] private Tile underTile;
    [SerializeField] private Biome[] allBiomes;

    private void Start()
    {
        Instantiate(startBiome, Vector3Int.zero, Quaternion.identity, gameObject.transform);
    }
    
    public void GenerateBiome(Vector3Int centerPosInt)
    {
        GameObject selectedBiome = SelectBiome();
        Instantiate(selectedBiome, centerPosInt, Quaternion.identity, gameObject.transform);
    }

    private GameObject SelectBiome()
    {
        int appearanceMax = 0;
        foreach (Biome b in allBiomes)
        {
            appearanceMax += b.appearanceRate;
        }

        int randomNum = Random.Range(0, appearanceMax);

        foreach (Biome b in allBiomes)
        {
            randomNum -= b.appearanceRate;
            if (randomNum < 0)
                return b.gameObject;
        }

        Debug.Log("バイオームの抽選エラー");
        return null;
    }
}
