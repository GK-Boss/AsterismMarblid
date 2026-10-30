using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDictionary : MonoBehaviour
{
    static public TileDictionary instance;

    [SerializeField] private List<Tile> allTiles;

    private Dictionary<string, Tile> stringToTile = new();

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Tile GetTile(string tileName)
    {
        if (!stringToTile.ContainsKey(tileName))
        {
            foreach (var tile in allTiles)
            {
                if (tileName == tile.tileBase.name)
                {
                    stringToTile[tileName] = tile;
                    break;
                }
            }
        }

        return stringToTile[tileName];
    }
}
