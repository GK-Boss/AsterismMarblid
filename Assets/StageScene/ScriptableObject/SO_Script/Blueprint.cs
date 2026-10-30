using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BluePrint")]
public class Blueprint : RewardBase
{
    [SerializeField] private BuildingData unLockBuilding;
    public override void Active()
    {
        unLockBuilding.unLoaked = true;
    }
}
