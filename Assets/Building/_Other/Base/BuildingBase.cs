using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingBase : MonoBehaviour, IChangeHP
{
    public BuildingData data;
    [SerializeField] private BuildingFillter fillter;
    [SerializeField] private bool changeDirection;
    [SerializeField] private bool rotateOffset;
    public float hp;
    private BuildCost[] payed;

    private BoxCollider2D col;
    public MoreDirection direction;

    public enum BuildState
    {
        Ghost, Build, Built, WillDestroy, Destroy
    }
    public BuildState buildState = BuildState.Ghost;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
        payed = new BuildCost[data.currentBuildingCosts.Length];

        transform.localScale = new Vector3(data.currentBuildingSize, data.currentBuildingSize);
        MapManager.instance.RegisterBuilding(this, false);

        // BuildingModule‚Ì’Ç‰Á
        if (data.useInventory)
            AddBuildingModule<InventoryModule>();
        if (data.useTurret)
            AddBuildingModule<TurretModule>();
        if (data.useMining)
            AddBuildingModule<MiningModule>();
    }

    private void AddBuildingModule<T>() where T : BuildingModule
    {
        T component = GetComponent<T>();
        if (!component)
            component = gameObject.AddComponent<T>();
        component.SetBuildingData(data);
    }
    private void Start()
    {
        fillter.ChangeFillter(buildState);
    }
    public void ChangeBuildState(BuildState nextState)
    {
        fillter.ChangeFillter(nextState);

        switch (nextState)
        {
            //case BuildState.Ghost:
            //    break;
            case BuildState.Build:
                MapManager.instance.RegisterBuilding(this, true);
                col.enabled = true;
                buildState = BuildState.Build;
                break;
            case BuildState.Built:
                buildState = BuildState.Built;
                break;
            case BuildState.WillDestroy:
                buildState = BuildState.WillDestroy;
                break;
            case BuildState.Destroy:
                buildState = BuildState.Destroy;
                break;
        }
    }
    public void SkipBuild()
    {
        hp = data.currentHp;
        MapManager.instance.RegisterBuilding(this, true);
        col.enabled = true;
        buildState = BuildState.Built;
        //fillter.ChangeFillter(BuildState.Built);
    }
    public void ChangeDirection(MoreDirection _direction)
    {
        if (!changeDirection) return;
        direction = _direction;
        switch (_direction)
        {
            case MoreDirection.Right:
                if (rotateOffset)
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                else
                    transform.localRotation = Quaternion.Euler(0, 0, -90);
                break;
            case MoreDirection.Left:
                if (rotateOffset)
                    transform.localRotation = Quaternion.Euler(0, 0, 180);
                else
                    transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case MoreDirection.Up:
                if (rotateOffset)
                    transform.localRotation = Quaternion.Euler(0, 0, 90);
                else
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case MoreDirection.Down:
                if (rotateOffset)
                    transform.localRotation = Quaternion.Euler(0, 0, -90);
                else
                    transform.localRotation = Quaternion.Euler(0, 0, 180);
                break;
            default:
                break;
        }
    }

    public void Progress()
    {
        switch (buildState)
        {
            case BuildState.Build:
                hp += Time.deltaTime / data.currentBuildTime;
                if (hp < 1f)
                {
                    for (int i = 0; i < data.currentBuildingCosts.Length; i++)
                    {
                        BuildCost cost = data.currentBuildingCosts[i];
                        int pastPayed = (int)payed[i].amount;
                        payed[i].amount += cost.amount / data.currentBuildTime * Time.deltaTime;
                        int nextPay = (int)payed[i].amount;
                        if (nextPay > pastPayed)
                        {
                            if (nextPay > cost.amount)
                            {
                                ResourceManager.instance.ChangeResourceNum(cost.resource, pastPayed - (int)cost.amount);
                                payed[i].amount = cost.amount;
                            }
                            else
                                ResourceManager.instance.ChangeResourceNum(cost.resource, pastPayed - nextPay);
                        }
                    }
                }
                else
                {
                    hp = data.currentHp;
                    for (int i = 0; i < data.currentBuildingCosts.Length; i++)
                    {
                        BuildCost cost = data.currentBuildingCosts[i];
                        ResourceManager.instance.ChangeResourceNum(cost.resource, (int)payed[i].amount - (int)cost.amount);
                        payed[i].amount = cost.amount;
                    }

                    ChangeBuildState(BuildState.Built);
                }
                break;
            case BuildState.Destroy:
                hp -= data.currentHp / data.currentBuildTime * Time.deltaTime;
                if (hp <= 0)
                {
                    Delete();
                }
                break;
            default:
                break;
        }
    }

    public Direction GetDirection()
    {
        switch (direction)
        {
            case MoreDirection.Right:
                return Direction.Right;
            case MoreDirection.Left:
                return Direction.Left;
            case MoreDirection.Up:
                return Direction.Up;
            case MoreDirection.Down:
                return Direction.Down;
            default:
                return Direction.Right;
        }
    }
    public void ChangeHP(float value)
    {
        hp += value;

        if (hp <= 0)
        {
            Delete();
        }
    }
    public void Delete()
    {
        MapManager.instance.UnRegisterBuilding(this);
        Destroy(gameObject);
    }

}
