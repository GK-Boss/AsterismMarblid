using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildMode : MonoBehaviour
{
    [SerializeField] private SpriteRenderer buildGhost;
    private Vector3Int mousePosInt;
    private Vector3Int startMousePosInt;
    private BuildingData pastSelectedBuilding;
    private BuildingData selectedBuilding;

    private BuildingBase startBuilding;
    private int pastDistance;
    private Stack<BuildingBase> willBuildStack = new();
    private List<Vector3Int> builtPoint = new();
    private List<BuildingBase> willBuild = new();
    private List<BuildingBase> willDestroy = new();
    private MoreDirection pastBuildDirection = MoreDirection.Right;
    private MoreDirection buildDirection = MoreDirection.Right;
    private bool stopConider;
    private int selectedSize;

    MoreDirection startMouseDirection;

    void Update()
    {
        selectedBuilding = BuildingSelectCtrl.instance.selectBuilding;
        MapManager.instance.SetBuildNum(willBuild.Count);
        if (selectedBuilding)
            Build();
    }
    private void Build()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePosInt = Vector3Int.RoundToInt(mousePos);
        selectedSize = selectedBuilding.currentBuildingSize;
        buildGhost.enabled = false;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            KeyDown();
        else if (Input.GetKey(KeyCode.Mouse0))
            KeyDownning();
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            KeyUp();
        else
            Idling();
    }
    private void KeyDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (CheckUI())
        {
            stopConider = true;
            return;
        }

        startMousePosInt = mousePosInt;

        if (mousePos.x % 1 <= 0.5f * Mathf.Sign(mousePos.x))
            if (mousePos.y % 1 <= 0.5f * Mathf.Sign(mousePos.y))
                startMouseDirection = MoreDirection.RightUp;
            else
                startMouseDirection = MoreDirection.RightDown;
        else
            if (mousePos.y % 1 <= 0.5f * Mathf.Sign(mousePos.y))
            startMouseDirection = MoreDirection.LeftUp;
        else
            startMouseDirection = MoreDirection.LeftDown;

        BuildingBase overlapBuilding = MapManager.instance.GetBuilding(mousePosInt);
        if (overlapBuilding && !overlapBuilding.data.isStatic)
        {
            switch (overlapBuilding.buildState)
            {
                case BuildingBase.BuildState.Ghost://削除
                    overlapBuilding.Delete();
                    willBuild.Remove(overlapBuilding);
                    stopConider = true;
                    break;
                case BuildingBase.BuildState.WillDestroy://解除
                    overlapBuilding.ChangeBuildState(BuildingBase.BuildState.Built);
                    willDestroy.Remove(overlapBuilding);
                    stopConider = true;
                    break;
                case BuildingBase.BuildState.Destroy://解除
                    overlapBuilding.ChangeBuildState(BuildingBase.BuildState.Built);
                    stopConider = true;
                    break;
            }
        }
        else if (Build_CheckFirst(mousePosInt, selectedSize))//建築
        {
            if (selectedSize % 2 == 0)
            {
                Vector3 buildPos = mousePosInt;
                switch (startMouseDirection)
                {
                    case MoreDirection.RightUp:
                        buildPos.x += 0.5f;
                        buildPos.y += 0.5f;
                        break;
                    case MoreDirection.RightDown:
                        buildPos.x += 0.5f;
                        buildPos.y -= 0.5f;
                        break;
                    case MoreDirection.LeftUp:
                        buildPos.x -= 0.5f;
                        buildPos.y += 0.5f;
                        break;
                    case MoreDirection.LeftDown:
                        buildPos.x -= 0.5f;
                        buildPos.y -= 0.5f;
                        break;
                }
                BuildingBase building = Instantiate(selectedBuilding.prefab, buildPos, Quaternion.identity).GetComponent<BuildingBase>();
                startBuilding = building;
                willBuild.Add(building);
            }
            else
            {
                BuildingBase building = Instantiate(selectedBuilding.prefab, mousePosInt, Quaternion.identity).GetComponent<BuildingBase>();
                startBuilding = building;
                willBuild.Add(building);
            }
        }
    }
    private void KeyDownning()
    {
        if (stopConider) return;

        //方向変換
        Vector3Int vec = mousePosInt - startMousePosInt;
        if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            if (vec.x > 0)
                buildDirection = MoreDirection.Right;
            else
                buildDirection = MoreDirection.Left;
        else if (Mathf.Abs(vec.x) < Mathf.Abs(vec.y))
            if (vec.y > 0)
                buildDirection = MoreDirection.Up;
            else
                buildDirection = MoreDirection.Down;
        startBuilding?.ChangeDirection(buildDirection);

        //方向が変わったときに、長押し部分の削除
        if (buildDirection != pastBuildDirection || mousePosInt == startMousePosInt)
        {
            foreach (BuildingBase building in willBuildStack)
                building.Delete();
            willBuildStack.Clear();
            builtPoint.Clear();
            pastDistance = 0;
            pastBuildDirection = buildDirection;
        }
        //方向が変わっていなかったら次

        //mousePos = mousePosInt;
        int nowDistance = Mathf.Max(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
        int difference = nowDistance - pastDistance;
        pastDistance = nowDistance;
        if (difference > 0) // 新規建築
        {
            //新しいマウス位置まで、初めのところから差の回数だけ各マスを調査して建築
            switch (buildDirection)
            {
                case MoreDirection.Right:
                    for (int i = -difference + 1; i <= 0; i++)
                        Build_Build(new Vector3Int(mousePosInt.x + i, startMousePosInt.y), selectedSize);
                    break;
                case MoreDirection.Left:
                    for (int i = -difference + 1; i <= 0; i++)
                        Build_Build(new Vector3Int(mousePosInt.x - i, startMousePosInt.y), selectedSize);
                    break;
                case MoreDirection.Up:
                    for (int i = -difference + 1; i <= 0; i++)
                        Build_Build(new Vector3Int(startMousePosInt.x, mousePosInt.y + i), selectedSize);
                    break;
                case MoreDirection.Down:
                    for (int i = -difference + 1; i <= 0; i++)
                        Build_Build(new Vector3Int(startMousePosInt.x, mousePosInt.y - i), selectedSize);
                    break;
            }
        }
        else if (difference < 0) // 長押しの削除
        {
            switch (buildDirection)
            {
                case MoreDirection.Right:
                    for (int i = 0; i < -difference; i++)
                        Build_Delete(new Vector3Int(mousePosInt.x + i + 1, startMousePosInt.y));
                    break;
                case MoreDirection.Left:
                    for (int i = 0; i < -difference; i++)
                        Build_Delete(new Vector3Int(mousePosInt.x - i - 1, startMousePosInt.y));
                    break;
                case MoreDirection.Up:
                    for (int i = 0; i < -difference; i++)
                        Build_Delete(new Vector3Int(startMousePosInt.x, mousePosInt.y + i + 1));
                    break;
                case MoreDirection.Down:
                    for (int i = 0; i < -difference; i++)
                        Build_Delete(new Vector3Int(startMousePosInt.x, mousePosInt.y - i - 1));
                    break;
            }
        }

    }
    private void KeyUp()
    {
        foreach (BuildingBase next in willBuildStack)
        {
            willBuild.Add(next);
        }
        startBuilding = null;
        willBuildStack.Clear();
        builtPoint.Clear();
        pastDistance = 0;
    }
    private void Idling()
    {
        if (MapManager.instance.GetCanBuild(mousePosInt) && !CheckUI())
        {
            if (MapManager.instance.GetBuilding(mousePosInt))
            {
                buildGhost.color = new Color(1, 0, 0, 0.5f);
                buildGhost.enabled = true;
            }
            else
            {
                buildGhost.color = new Color(1, 1, 1, 0.5f);
                buildGhost.enabled = true;
            }
        }
        else
            buildGhost.enabled = false;

        buildGhost.transform.position = mousePosInt;
        if (selectedBuilding && pastSelectedBuilding != selectedBuilding)
        {
            buildGhost.sprite = selectedBuilding.ghost;
            pastSelectedBuilding = selectedBuilding;
        }
        stopConider = false;
    }

    private bool Build_CheckFirst(Vector3Int centerPos, int size)
    {
        centerPos.z = 0;
        if (size % 2 == 0) // 偶数
        {
            switch (startMouseDirection)
            {
                case MoreDirection.RightUp:
                    for (int x = -size / 2 + 1; x <= size / 2; x++)
                        for (int y = -size / 2 + 1; y <= size / 2; y++)
                            if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(x, y, 0))) return false;
                    return true;
                case MoreDirection.RightDown:
                    for (int x = -size / 2 + 1; x <= size / 2; x++)
                        for (int y = -size / 2 + 1; y <= size / 2; y++)
                            if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(x, -y, 0))) return false;
                    return true;
                case MoreDirection.LeftUp:
                    for (int x = -size / 2 + 1; x <= size / 2; x++)
                        for (int y = -size / 2 + 1; y <= size / 2; y++)
                            if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(-x, y, 0))) return false;
                    return true;
                case MoreDirection.LeftDown:
                    for (int x = -size / 2 + 1; x <= size / 2; x++)
                        for (int y = -size / 2 + 1; y <= size / 2; y++)
                            if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(-x, -y, 0))) return false;
                    return true;
            }
            Debug.Log("建築初期条件エラー, " + startMouseDirection);
            return false;
        }
        else // 奇数
        {
            for (int x = -size / 2; x <= size / 2; x++)
                for (int y = -size / 2; y <= size / 2; y++)
                    if (!MapManager.instance.GetCanBuild(Vector3Int.RoundToInt(centerPos) + new Vector3Int(x, y, 0))) return false;
            return true;
        }
    }
    private bool Build_Check(Vector3Int centerPos, int size)
    {
        centerPos.z = 0;
        if (size % 2 == 0) // 偶数
        {
            switch (buildDirection)
            {
                case MoreDirection.Right:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.LeftUp:
                            for (int x = 0; x < size; x++)
                                for (int y = -size / 2 + 1; y <= size / 2; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(x, y, 0))) return false;
                            return true;
                        case MoreDirection.RightDown:
                        case MoreDirection.LeftDown:
                            for (int x = 0; x < size; x++)
                                for (int y = -size / 2 + 1; y <= size / 2; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(x, -y, 0))) return false;
                            return true;
                    }
                    break;
                case MoreDirection.Left:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.LeftUp:
                            for (int x = 0; x < size; x++)
                                for (int y = -size / 2 + 1; y <= size / 2; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(-x, y, 0))) return false;
                            return true;
                        case MoreDirection.RightDown:
                        case MoreDirection.LeftDown:
                            for (int x = 0; x < size; x++)
                                for (int y = -size / 2 + 1; y <= size / 2; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(-x, -y, 0))) return false;
                            return true;
                    }
                    break;
                case MoreDirection.Up:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.RightDown:
                            for (int x = -size / 2 + 1; x <= size / 2; x++)
                                for (int y = 0; y < size; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(x, y, 0))) return false;
                            return true;
                        case MoreDirection.LeftUp:
                        case MoreDirection.LeftDown:
                            for (int x = -size / 2 + 1; x <= size / 2; x++)
                                for (int y = 0; y < size; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(-x, y, 0))) return false;
                            return true;
                    }
                    break;
                case MoreDirection.Down:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.RightDown:
                            for (int x = -size / 2 + 1; x <= size / 2; x++)
                                for (int y = 0; y < size; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(x, -y, 0))) return false;
                            return true;
                        case MoreDirection.LeftUp:
                        case MoreDirection.LeftDown:
                            for (int x = -size / 2 + 1; x <= size / 2; x++)
                                for (int y = 0; y < size; y++)
                                    if (!MapManager.instance.GetCanBuild(centerPos + new Vector3Int(-x, -y, 0))) return false;
                            return true;
                    }
                    break;
            }

            Debug.Log("建築条件エラー, " + buildDirection);
            return false;
        }
        else // 奇数
        {
            for (int x = -size / 2; x <= size / 2; x++)
                for (int y = -size / 2; y <= size / 2; y++)
                    if (!MapManager.instance.GetCanBuild(Vector3Int.RoundToInt(centerPos) + new Vector3Int(x, y, 0))) return false;
            return true;
        }
    }
    private void Build_Build(Vector3Int buildPosInt, int size)
    {
        Vector3 buildPos = buildPosInt;
        if (!Build_Check(buildPosInt, size)) return;

        if (size % 2 == 0) // 偶数
        {
            builtPoint.Add(Vector3Int.RoundToInt(buildPos));

            //位置調整
            switch (buildDirection)
            {
                case MoreDirection.Right:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.LeftUp:
                            buildPos.x += 0.5f + (size / 2 - 1);
                            buildPos.y += 0.5f;
                            break;
                        case MoreDirection.RightDown:
                        case MoreDirection.LeftDown:
                            buildPos.x += 0.5f + (size / 2 - 1);
                            buildPos.y -= 0.5f;
                            break;
                    }
                    break;
                case MoreDirection.Left:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.LeftUp:
                            buildPos.x -= 0.5f + (size / 2 - 1);
                            buildPos.y += 0.5f;
                            break;
                        case MoreDirection.RightDown:
                        case MoreDirection.LeftDown:
                            buildPos.x -= 0.5f + (size / 2 - 1);
                            buildPos.y -= 0.5f;
                            break;
                    }
                    break;
                case MoreDirection.Up:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.RightDown:
                            buildPos.x += 0.5f;
                            buildPos.y += 0.5f + (size / 2 - 1);
                            break;
                        case MoreDirection.LeftUp:
                        case MoreDirection.LeftDown:
                            buildPos.x -= 0.5f;
                            buildPos.y += 0.5f + (size / 2 - 1);
                            break;
                    }
                    break;
                case MoreDirection.Down:
                    switch (startMouseDirection)
                    {
                        case MoreDirection.RightUp:
                        case MoreDirection.RightDown:
                            buildPos.x += 0.5f;
                            buildPos.y -= 0.5f + (size / 2 - 1);
                            break;
                        case MoreDirection.LeftUp:
                        case MoreDirection.LeftDown:
                            buildPos.x -= 0.5f;
                            buildPos.y -= 0.5f + (size / 2 - 1);
                            break;
                    }
                    break;
            }

            BuildingBase building = Instantiate(selectedBuilding.prefab, buildPos, Quaternion.identity).GetComponent<BuildingBase>();
            building.ChangeDirection(buildDirection);
            willBuildStack.Push(building);
        }
        else // 奇数
        {
            BuildingBase building = Instantiate(selectedBuilding.prefab, buildPos, Quaternion.identity).GetComponent<BuildingBase>();

            switch (buildDirection)
            {
                case MoreDirection.Right:
                    buildPos.x -= size / 2;
                    break;
                case MoreDirection.Left:
                    buildPos.x += size / 2;
                    break;
                case MoreDirection.Up:
                    buildPos.y -= size / 2;
                    break;
                case MoreDirection.Down:
                    buildPos.y += size / 2;
                    break;
            }
            builtPoint.Add(Vector3Int.RoundToInt(buildPos));
            building.ChangeDirection(buildDirection);
            willBuildStack.Push(building);
        }
    }
    private void Build_Delete(Vector3Int pos)
    {
        if (!builtPoint.Contains(Vector3Int.RoundToInt(pos))) return;
        BuildingBase building = willBuildStack.Pop();
        builtPoint.Remove(Vector3Int.RoundToInt(pos));
        building.Delete();
    }
    public void Diside()
    {
        foreach (BuildingBase building in willBuild)
        {
            if (building)
                building.ChangeBuildState(BuildingBase.BuildState.Build);
        }
        willBuild.Clear();

        MapManager.instance.SetBuildNum(0);
    }

    private bool CheckUI()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return true;

        List<RaycastResult> raycastResults = new();
        if (!EventSystem.current) return false;

        PointerEventData pointerEventData = new(EventSystem.current) { position = Input.mousePosition };
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count == 0) return false;

        return raycastResults[0].gameObject.layer == 5;
    }
}
