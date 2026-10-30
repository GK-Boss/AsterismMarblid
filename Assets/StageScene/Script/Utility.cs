
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// 建物の中心座標とサイズから、占有するすべてのセル座標を取得する。
    ///
    /// size = 1 : 1x1
    /// size = 2 : 2x2
    /// size = 3 : 3x3
    /// size = 4 : 4x4
    ///
    /// 奇数サイズは整数座標が中心。
    /// 偶数サイズは半整数座標が中心。
    /// </summary>
    public static List<Vector3Int> PositionAndSizeToAllPosInt(
        Vector3 position,
        int size)
    {
        List<Vector3Int> allPosIntList = new();

        if (size <= 0)
        {
            Debug.LogError($"Building size is invalid: {size}");
            return allPosIntList;
        }

        // 左下のセルを取得
        int startX = Mathf.CeilToInt(position.x - size * 0.5f);
        int startY = Mathf.CeilToInt(position.y - size * 0.5f);

        int z = Mathf.RoundToInt(position.z);

        // サイズ分のセルを登録
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                allPosIntList.Add(
                    new Vector3Int(
                        startX + x,
                        startY + y,
                        z
                    )
                );
            }
        }

        return allPosIntList;
    }

    #region Direction

    public static Vector3Int DirectionToVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right:
                return Vector3Int.right;

            case Direction.Left:
                return Vector3Int.left;

            case Direction.Up:
                return Vector3Int.up;

            case Direction.Down:
                return Vector3Int.down;
        }

        return Vector3Int.zero;
    }

    public static Direction WorldToLocal(
        Direction world,
        Direction buildingDir)
    {
        int w = (int)world;
        int b = (int)buildingDir;

        return (Direction)((w - b + 4) % 4);
    }

    public static Direction LocalToWorld(
        Direction local,
        Direction buildingDir)
    {
        int l = DirToIndex(local);
        int b = DirToIndex(buildingDir);

        return IndexToDir((l + b) % 4);
    }

    public static Direction Opposite(Direction dir)
    {
        return IndexToDir((DirToIndex(dir) + 2) % 4);
    }

    public static int DirToIndex(Direction dir)
    {
        switch (dir)
        {
            case Direction.Right:
                return 0;

            case Direction.Up:
                return 1;

            case Direction.Left:
                return 2;

            case Direction.Down:
                return 3;
        }

        return 0;
    }

    public static Direction IndexToDir(int index)
    {
        switch (index)
        {
            case 0:
                return Direction.Right;

            case 1:
                return Direction.Up;

            case 2:
                return Direction.Left;

            case 3:
                return Direction.Down;
        }

        return Direction.Right;
    }

    #endregion

    #region MoreDirection

    public static Vector3Int MoreDirectionToVector(MoreDirection dir)
    {
        switch (dir)
        {
            case MoreDirection.Up:
                return Vector3Int.up;

            case MoreDirection.Down:
                return Vector3Int.down;

            case MoreDirection.Left:
                return Vector3Int.left;

            case MoreDirection.Right:
                return Vector3Int.right;
        }

        return Vector3Int.zero;
    }

    public static MoreDirection Rotate(
        MoreDirection local,
        MoreDirection buildingDir)
    {
        int localInt = ToInt(local);
        int rotInt = ToInt(buildingDir);

        int result = (localInt + rotInt) % 4;

        return FromInt(result);
    }

    static int ToInt(MoreDirection dir)
    {
        switch (dir)
        {
            case MoreDirection.Up: return 0;
            case MoreDirection.Right: return 1;
            case MoreDirection.Down: return 2;
            case MoreDirection.Left: return 3;
        }

        return 0;
    }

    static MoreDirection FromInt(int i)
    {
        switch (i % 4)
        {
            case 0: return MoreDirection.Up;
            case 1: return MoreDirection.Right;
            case 2: return MoreDirection.Down;
            case 3: return MoreDirection.Left;
        }

        return MoreDirection.Up;
    }

    #endregion
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}

public enum MoreDirection
{
    Right,
    Left,
    Up,
    Down,
    RightUp,
    RightDown,
    LeftUp,
    LeftDown
}
