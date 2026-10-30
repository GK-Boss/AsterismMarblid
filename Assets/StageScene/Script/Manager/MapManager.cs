
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

    // 建物によって移動コストが変更されているセル
    private HashSet<Vector3Int> buildingCostBlockedCells = new();

    private int buildNum;
    private int destroyNum;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Tile

    public void RegisterTile(Vector3Int posInt, Tile tile)
    {
        if (tileMap.ContainsKey(posInt))
            tileMap[posInt] = tile;
        else
            tileMap.Add(posInt, tile);

        // 建物が存在するセルなら移動コストを1000に維持
        if (buildingCostBlockedCells.Contains(posInt))
        {
            costMap[posInt] = 1000;
        }
        else
        {
            costMap[posInt] = tile.cost;
        }

        if (enemyNav)
            enemyNav.NotifyMapChanged(posInt);
    }

    public void RegisterVein(Vector3Int posInt, Vein vein)
    {
        if (veinMap.ContainsKey(posInt))
            veinMap[posInt] = vein;
        else
            veinMap.Add(posInt, vein);
    }

    #endregion

    #region Building

    /// <summary>
    /// 建物をマップに登録する。
    /// changeCost = false の場合は占有情報のみ登録する。
    /// changeCost = true の場合は移動コストも変更する。
    /// </summary>
    public void RegisterBuilding(
        BuildingBase building,
        bool changeCost)
    {
        if (!building || !building.data)
        {
            Debug.LogError("RegisterBuilding: Building or BuildingData is null.");
            return;
        }

        List<Vector3Int> posInts =
            Utility.PositionAndSizeToAllPosInt(
                building.transform.position,
                building.data.currentBuildingSize
            );

        foreach (Vector3Int posInt in posInts)
        {
            RegisterBuilding(posInt, building, changeCost);
        }
    }

    private void RegisterBuilding(
        Vector3Int posInt,
        BuildingBase building,
        bool changeCost)
    {
        // 建物の占有情報を登録
        if (buildingMap.ContainsKey(posInt))
        {
            BuildingBase current = buildingMap[posInt];

            // 別の建物が存在する場合は警告
            if (current != null && current != building)
            {
                Debug.LogWarning(
                    $"Building overlap detected at {posInt}: " +
                    $"{current.name} / {building.name}"
                );
            }
        }

        buildingMap[posInt] = building;

        // ゴースト状態などでは移動コストを変更しない
        if (!changeCost)
            return;

        // 移動コストを変更
        buildingCostBlockedCells.Add(posInt);
        costMap[posInt] = 1000;

        if (enemyNav)
            enemyNav.NotifyMapChanged(posInt);
    }

    /// <summary>
    /// 建物をマップから解除する。
    /// </summary>
    public void UnRegisterBuilding(BuildingBase building)
    {
        if (!building || !building.data)
            return;

        List<Vector3Int> posInts =
            Utility.PositionAndSizeToAllPosInt(
                building.transform.position,
                building.data.currentBuildingSize
            );

        foreach (Vector3Int posInt in posInts)
        {
            UnRegisterBuilding(posInt, building);
        }
    }

    private void UnRegisterBuilding(
        Vector3Int posInt,
        BuildingBase building)
    {
        // このセルを占有しているのが対象の建物か確認
        if (buildingMap.TryGetValue(posInt, out BuildingBase current))
        {
            if (current == building)
            {
                buildingMap[posInt] = null;
            }
        }

        // 移動コストを変更していたセルだけ復元する
        if (!buildingCostBlockedCells.Remove(posInt))
            return;

        // 元の地形コストに戻す
        if (tileMap.TryGetValue(posInt, out Tile tile) && tile != null)
        {
            costMap[posInt] = tile.cost;
        }
        else
        {
            costMap.Remove(posInt);
        }

        if (enemyNav)
            enemyNav.NotifyMapChanged(posInt);
    }

    #endregion

    #region Build / Destroy

    public void SetBuildNum(int num)
    {
        buildNum = num;
    }

    public void SetDestroyNum(int num)
    {
        destroyNum = num;
    }

    public bool GetCanDiside()
    {
        return buildNum != 0 || destroyNum != 0;
    }

    #endregion

    #region Get

    public Tile GetTile(Vector3Int posInt)
    {
        if (tileMap.TryGetValue(posInt, out Tile tile))
            return tile;

        return null;
    }

    public Vein GetVein(Vector3Int posInt)
    {
        if (veinMap.TryGetValue(posInt, out Vein vein))
            return vein;

        return null;
    }

    public BuildingBase GetBuilding(Vector3Int posInt)
    {
        if (buildingMap.TryGetValue(posInt, out BuildingBase building))
            return building;

        return null;
    }

    /// <summary>
    /// 指定した1セルに建物を建築できるか確認する。
    /// </summary>
    public bool GetCanBuild(Vector3Int posInt)
    {
        if (!tileMap.TryGetValue(posInt, out Tile tile))
            return false;

        if (!tile || !tile.canWalk)
            return false;

        if (buildingMap.TryGetValue(posInt, out BuildingBase building))
        {
            if (building != null)
                return false;
        }

        return true;
    }

    public bool GetCanWalk(Vector3Int posInt)
    {
        Tile tile = GetTile(posInt);

        if (tile)
            return tile.canWalk;

        return false;
    }

    public float GetCost(Vector3Int posInt)
    {
        if (costMap.TryGetValue(posInt, out float cost))
            return cost;

        return 0;
    }

    public bool HasTile(Vector3Int posInt)
    {
        return tileMap.ContainsKey(posInt);
    }

    #endregion

    #region Change Tile

    public void ChangeTile(Vector3Int posInt, Tile tile)
    {
        tilemap.SetTile(posInt, tile.tileBase);
    }

    public void ChangeVein(Vector3Int posInt, Vein vein)
    {
        veinmap.SetTile(posInt, vein.tileBase);
    }

    #endregion

    #region Generate

    public void GenerateBiome(Vector3Int centerPosInt)
    {
        biomeGenerator.GenerateBiome(centerPosInt);
    }

    #endregion
}
