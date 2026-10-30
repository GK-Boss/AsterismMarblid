using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigation : MonoBehaviour
{
    public static EnemyNavigation instance;

    private struct Key : System.IComparable<Key>
    {
        public float k1;
        public float k2;

        public int CompareTo(Key other)
        {
            int result = k1.CompareTo(other.k1);
            if (result == 0) return k2.CompareTo(other.k2);
            return result;
        }
    }

    private class Cell
    {
        public Vector3Int pos;
        public float g = float.PositiveInfinity;
        public float rhs = float.PositiveInfinity;
    }

    private Dictionary<Vector3Int, Cell> cells = new();
    private PriorityQueue<Vector3Int, Key> openList = new();
    private HashSet<Vector3Int> inOpenList = new();

    private Vector3Int goalPos;
    private readonly Vector3Int[] directions = {
        new(0, 1, 0), new(0, -1, 0), new(1, 0, 0), new(-1, 0, 0),
        new(1, 1, 0), new(1, -1, 0), new(-1, 1, 0), new(-1, -1, 0)
    };

    [SerializeField] private int nodesPerFrame = 300;
    private bool isComputing = false;

    private void Awake() => instance = this;

    private void Start()
    {
        SetGoal(Vector3Int.zero);
    }

    public void SetGoal(Vector3Int newGoal)
    {
        cells.Clear();
        openList.Clear();
        inOpenList.Clear();

        goalPos = newGoal;
        Cell goal = GetCell(goalPos);
        goal.rhs = 0;

        AddToOpenList(goalPos, CalculateKey(goal));

        if (!isComputing)
            StartCoroutine(ComputeShortestPathCoroutine());
    }

    public Vector3Int GetNext(Vector3Int currentPos)
    {
        Vector3Int bestStep = currentPos;
        float minTotalCost = float.PositiveInfinity;

        foreach (var dir in directions)
        {
            Vector3Int neighborPos = currentPos + dir;
            Cell neighbor = GetCell(neighborPos);
            float moveCost = CalculateMoveCost(currentPos, neighborPos);

            if (float.IsInfinity(moveCost)) continue;

            float totalCost = neighbor.g + moveCost;
            if (totalCost < minTotalCost)
            {
                minTotalCost = totalCost;
                bestStep = neighborPos;
            }
        }
        return bestStep;
    }

    public void NotifyMapChanged(Vector3Int changedPos)
    {
        UpdateState(changedPos);
        foreach (var dir in directions) UpdateState(changedPos + dir);

        if (!isComputing)
            StartCoroutine(ComputeShortestPathCoroutine());
    }

    private void UpdateState(Vector3Int pos)
    {
        Cell u = GetCell(pos);
        if (pos != goalPos)
        {
            float minRhs = float.PositiveInfinity;
            foreach (var dir in directions)
            {
                Cell v = GetCell(pos + dir);
                float moveCost = CalculateMoveCost(pos, pos + dir);

                if (float.IsInfinity(v.g) || float.IsInfinity(moveCost)) continue;

                float val = v.g + moveCost;
                if (val < minRhs) minRhs = val;
            }
            u.rhs = minRhs;
        }

        if (Mathf.Abs(u.g - u.rhs) > 0.001f)
        {
            AddToOpenList(pos, CalculateKey(u));
        }
        else
        {
            u.g = u.rhs;
        }
    }

    private IEnumerator ComputeShortestPathCoroutine()
    {
        isComputing = true;
        int processedNodes = 0;

        while (openList.Count > 0)
        {
            if (processedNodes >= nodesPerFrame)
            {
                processedNodes = 0;
                yield return null;
            }

            if (!openList.TryDequeue(out Vector3Int uPos, out Key kOld)) continue;
            inOpenList.Remove(uPos);

            Cell u = GetCell(uPos);
            Key kCurrent = CalculateKey(u);

            if (kOld.CompareTo(kCurrent) < 0)
            {
                if (Mathf.Abs(u.g - u.rhs) > 0.001f) AddToOpenList(uPos, kCurrent);
                continue;
            }

            processedNodes++;

            if (u.g > u.rhs)
            {
                u.g = u.rhs;
                foreach (var dir in directions) UpdateState(uPos + dir);
            }
            else
            {
                u.g = float.PositiveInfinity;
                UpdateState(uPos);
                foreach (var dir in directions) UpdateState(uPos + dir);
            }
        }

        isComputing = false;
    }

    private Key CalculateKey(Cell u)
    {
        float min = Mathf.Min(u.g, u.rhs);
        return new Key { k1 = min, k2 = min };
    }

    private void AddToOpenList(Vector3Int pos, Key key)
    {
        if (inOpenList.Contains(pos)) return;

        openList.Enqueue(pos, key);
        inOpenList.Add(pos);
    }

    private Cell GetCell(Vector3Int p)
    {
        if (!cells.TryGetValue(p, out Cell c))
        {
            c = new Cell { pos = p, g = float.PositiveInfinity, rhs = float.PositiveInfinity };
            cells[p] = c;
        }
        return c;
    }

    private float CalculateMoveCost(Vector3Int a, Vector3Int b)
    {
        if (!MapManager.instance.HasTile(b)) return float.PositiveInfinity;

        float baseCost = Mathf.Max(1.0f, MapManager.instance.GetCost(b));

        if (!MapManager.instance.HasTile(b)) return float.PositiveInfinity;
        if (!MapManager.instance.GetCanWalk(b)) return 500f;

        Vector3Int dir = b - a;
        bool isDiagonal = (dir.x != 0 && dir.y != 0);

        if (isDiagonal)
        {
            if (!MapManager.instance.HasTile(a + new Vector3Int(dir.x, 0, 0)) ||
                !MapManager.instance.HasTile(a + new Vector3Int(0, dir.y, 0)))
                return float.PositiveInfinity;

            if (!MapManager.instance.GetCanWalk(a + new Vector3Int(dir.x, 0, 0)) ||
                !MapManager.instance.GetCanWalk(a + new Vector3Int(0, dir.y, 0)))
                return 500f;

            return 1.414f * baseCost;
        }
        return baseCost;
    }

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying || cells == null) return;

    //    Camera cam = Camera.main;
    //    if (cam == null) return;

    //    float height = 2f * cam.orthographicSize;
    //    float width = height * cam.aspect;
    //    Vector3 camPos = cam.transform.position;
    //    Rect visibleRect = new Rect(camPos.x - width / 2, camPos.y - height / 2, width, height);

    //    foreach (var kvp in cells)
    //    {
    //        Vector3Int pos = kvp.Key;
    //        if (!visibleRect.Contains(new Vector2(pos.x, pos.y))) continue;

    //        Cell cell = kvp.Value;
    //        if (!float.IsInfinity(cell.g))
    //        {
    //            float lerp = Mathf.Clamp01(cell.g / 100f);
    //            Gizmos.color = Color.Lerp(Color.green, Color.red, lerp);
    //            Gizmos.DrawCube(new Vector3(pos.x, pos.y, 0), new Vector3(0.3f, 0.3f, 0.1f));

    //            Vector3Int next = GetNext(pos);
    //            if (next != pos)
    //            {
    //                Gizmos.color = Color.yellow;
    //                Gizmos.DrawLine(new Vector3(pos.x, pos.y, 0), new Vector3(next.x, next.y, 0));
    //            }
    //        }
    //    }
    //}
}