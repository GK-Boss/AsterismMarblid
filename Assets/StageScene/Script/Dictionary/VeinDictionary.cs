using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinDictionary : MonoBehaviour
{
    static public VeinDictionary instance;

    [SerializeField] private List<Vein> allTiles;

    private Dictionary<string, Vein> stringToVein = new();

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    public Vein GetVein(string tileName)
    {
        if (!stringToVein.ContainsKey(tileName))
        {
            foreach (var tile in allTiles)
            {
                if (tileName == tile.tileBase.name)
                {
                    stringToVein[tileName] = tile;
                    break;
                }
            }
        }

        return stringToVein[tileName];
    }

}
