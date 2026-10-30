using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardBoxView : MonoBehaviour
{
    public Image icon;
    public Text text;

    [HideInInspector] public RewardBox box;


    public void Select()
    {
        RewardBoxPanel.instance.SelectRewardBox(box, gameObject);
    }
}
