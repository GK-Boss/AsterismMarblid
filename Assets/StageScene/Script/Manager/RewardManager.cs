using UnityEngine;
using System.Collections.Generic;

public class RewardManager : MonoBehaviour
{
    static public RewardManager instance;

    [SerializeField] private List<RewardBox> rewards = new();

    private void Awake()
    {
        instance = this;
    }

    public List<RewardBox> GetRewardBoxList() => rewards;

    public void AddReward(RewardBox reward)
    {
        rewards.Add(reward);
    }

    public void RemoveReward(RewardBox reward)
    {
        rewards.Remove(reward);
    }
}
