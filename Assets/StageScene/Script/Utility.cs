using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Utility
{
    private static Dictionary<int, List<Vector3>> sizeToVector = new();
    public static List<Vector3Int> PositionAndSizeToAllPosInt(Vector3 position, int size)
    {
        List<Vector3Int> allPosIntList = new();

        if (!sizeToVector.ContainsKey(size))
        {
            List<Vector3> allVec = new();
            if (size % 2 == 0)
                for (float x = -size / 2.0f + 0.5f; x <= size / 2.0f - 0.5f; x++)
                    for (float y = -size / 2.0f + 0.5f; y <= size / 2.0f - 0.5f; y++)
                        allVec.Add(new(x, y));
            else
                for (int x = -size / 2; x <= size / 2; x++)
                    for (int y = -size / 2; y <= size / 2; y++)
                        allVec.Add(new(x, y));

            sizeToVector.Add(size, allVec);
        }

        foreach (var vec in sizeToVector[size])
        {
            allPosIntList.Add(Vector3Int.RoundToInt(position + vec));
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

    public static Direction WorldToLocal(Direction world, Direction buildingDir)
    {
        int w = (int)world;
        int b = (int)buildingDir;

        return (Direction)((w - b + 4) % 4);
    }
    public static Direction LocalToWorld(Direction local, Direction buildingDir)
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

    public static MoreDirection Rotate(MoreDirection local, MoreDirection buildingDir)
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
    Right, Left, Up, Down
}
public enum MoreDirection
{
    Right, Left, Up, Down, RightUp, RightDown, LeftUp, LeftDown
}
