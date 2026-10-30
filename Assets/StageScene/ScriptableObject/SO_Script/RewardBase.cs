using UnityEngine;
using UnityEngine.UI;

public abstract class RewardBase : ScriptableObject
{
    public Sprite rewardIcon;
    public Sprite underFrame;
    public Sprite topFrame;
    [TextArea] public string explanation;
    public RewardType type;
    public abstract void Active();
}

public enum RewardType
{
    Module, Expansion, Blueprint
}
