using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Reward")]
public class RewardBox : ScriptableObject
{
    public Sprite boxIcon;
    public RandomReward reward1;
    public RandomReward reward2;
    public RandomReward reward3;
    public string byName;
}

[System.Serializable]
public class RandomReward
{
    public Reward[] rewards;

}

[System.Serializable]
public class Reward
{
    public RewardBase reward;
    public int appearance;
}
