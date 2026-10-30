using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    static public MapManager instance;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap veinmap;

    [SerializeField] private BiomeGenerator biomeGenerator;
    [SerializeField] private EnemyNavigation enemyNav;
    
    private Dictionary<Vector3Int, Tile> tileMap = new();
    private Dictionary<Vector3Int, Vein> veinMap = new();
    private Dictionary<Vector3Int, BuildingBase> buildingMap = new();
    private Dictionary<Vector3Int, float> costMap = new();

    private int buildNum;
    private int destroyNum;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterTile(Vector3Int posInt, Tile tile)
    {
        if (tileMap.ContainsKey(posInt))
            tileMap[posInt] = tile;
        else
            tileMap.Add(posInt, tile);

        if (costMap.ContainsKey(posInt))
            costMap[posInt] = tile.cost;
        else
            costMap.Add(posInt, tile.cost);
        enemyNav.NotifyMapChanged(posInt);
    }
    public void RegisterVein(Vector3Int posInt, Vein vein)
    {
        if (veinMap.ContainsKey(posInt))
            veinMap[posInt] = vein;
        else
            veinMap.Add(posInt, vein);
    }
    public void RegisterBuilding(BuildingBase building, bool changeCost)
    {
        List<Vector3Int> posInts = Utility.PositionAndSizeToAllPosInt(building.transform.position, building.data.currentBuildingSize);
        foreach (var posInt in posInts)
        {
            RegisterBuilding(posInt, building, changeCost);
        }
    }
    private void RegisterBuilding(Vector3Int posInt, BuildingBase building, bool changeCost)
    {
        if (buildingMap.ContainsKey(posInt))
            buildingMap[posInt] = building;
        else
            buildingMap.Add(posInt, building);

        if (!changeCost) return;

        if (costMap.ContainsKey(posInt))
            costMap[posInt] = 1000;
        else
            costMap.Add(posInt, 1000);

        enemyNav.NotifyMapChanged(posInt);
    }

    public void UnRegisterBuilding(BuildingBase building)
    {
        int size = building.data.currentBuildingSize;

        if (size % 2 == 0)
            for (int x = Mathf.RoundToInt(building.transform.position.x - size / 2.0f + 0.5f); x <= Mathf.RoundToInt(building.transform.position.x + size / 2.0f - 0.5f); x++)
                for (int y = Mathf.RoundToInt(building.transform.position.y - size / 2.0f + 0.5f); y <= Mathf.RoundToInt(building.transform.position.y + size / 2.0f - 0.5f); y++)
                    UnRegisterBuilding(new Vector3Int(x, y), building);
        else
            for (int x = Vector3Int.RoundToInt(building.transform.position).x - size / 2; x <= Vector3Int.RoundToInt(building.transform.position).x + size / 2; x++)
                for (int y = Vector3Int.RoundToInt(building.transform.position).y - size / 2; y <= Vector3Int.RoundToInt(building.transform.position).y + size / 2; y++)
                    UnRegisterBuilding(new Vector3Int(x, y), building);
    }
    private void UnRegisterBuilding(Vector3Int posInt, BuildingBase building)
    {
        if (buildingMap.ContainsKey(posInt))
            buildingMap[posInt] = null;
        else
            Debug.Log("Error");

        if (costMap.ContainsKey(posInt))
            costMap[posInt] = tileMap[posInt].cost;
        else
            Debug.Log("Error");

        enemyNav.NotifyMapChanged(posInt);
    }


    public void SetBuildNum(int num)
    {
        buildNum = num;
    }
    public void SetDestroyNum(int num)
    {
        destroyNum = num;
    }

    public Tile GetTile(Vector3Int posInt)
    {
        if (tileMap.ContainsKey(posInt))
            return tileMap[posInt];
        else
            return null;
    }
    public Vein GetVein(Vector3Int posInt)
    {
        if (veinMap.ContainsKey(posInt))
            return veinMap[posInt];
        else
            return null;
    }
    public BuildingBase GetBuilding(Vector3Int posInt)
    {
        if (buildingMap.ContainsKey(posInt))
            return buildingMap[posInt];
        else
            return null;
    }
    public bool GetCanBuild(Vector3Int posInt)
    {
        if (tileMap.ContainsKey(posInt) && tileMap[posInt].canWalk)
        {
            if (!buildingMap.ContainsKey(posInt))
            {
                return true;
            }
            else
            {
                if (buildingMap[posInt] == null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool GetCanWalk(Vector3Int posInt)
    {
        Tile tile = GetTile(posInt);
        if (tile)
            return tile.canWalk;
        else
            return false;
    }
    public float GetCost(Vector3Int posInt)
    {
        if (costMap.ContainsKey(posInt))
        {
            return costMap[posInt];
        }
        else
            return 0;
        //return costMap[posInt];
    }

    public bool GetCanDiside() => buildNum != 0 || destroyNum != 0;
    public bool HasTile(Vector3Int posInt)
    {
        return tileMap.ContainsKey(posInt);
    }

    public void ChangeTile(Vector3Int posInt, Tile tile)
    {
        tilemap.SetTile(posInt, tile.tileBase);
    }
    public void ChangeVein(Vector3Int posInt, Vein vein)
    {
        veinmap.SetTile(posInt, vein.tileBase);
    }

    public void GenerateBiome(Vector3Int centerPosInt)
    {
        biomeGenerator.GenerateBiome(centerPosInt);
    }
}
