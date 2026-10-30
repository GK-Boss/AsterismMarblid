using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

//真ん中のパネル
public class RewardPanel : MonoBehaviour
{
    [SerializeField] private GameObject emphasisFrame;
    [SerializeField] private Text expanationText;

    [SerializeField] private Image content1;
    [SerializeField] private Image content2;
    [SerializeField] private Image content3;

    [SerializeField] private Text typeText1;
    [SerializeField] private Text typeText2;
    [SerializeField] private Text typeText3;

    [SerializeField] private Image underFrame1;
    [SerializeField] private Image underFrame2;
    [SerializeField] private Image underFrame3;

    [SerializeField] private Image topFrame1;
    [SerializeField] private Image topFrame2;
    [SerializeField] private Image topFrame3;

    private RewardBase leftReward;
    private RewardBase centerReward;
    private RewardBase rightReward;

    private int selectNum;

    public void SetBox(RewardBox box)
    {
        selectNum = 0;
        emphasisFrame.SetActive(false);
        expanationText.text = string.Empty;

        // 開封
        leftReward = OpenBox(box.reward1);
        centerReward = OpenBox(box.reward2);
        rightReward = OpenBox(box.reward3);

        content1.sprite = leftReward.rewardIcon;
        content2.sprite = centerReward.rewardIcon;
        content3.sprite = rightReward.rewardIcon;

        typeText1.text = leftReward.type.ToString();
        typeText2.text = centerReward.type.ToString();
        typeText3.text = rightReward.type.ToString();

        underFrame1.sprite = leftReward.underFrame;
        underFrame2.sprite = centerReward.underFrame;
        underFrame3.sprite = rightReward.underFrame;

        if (leftReward.topFrame)
        {
            topFrame1.enabled = true;
            topFrame1.sprite = leftReward.topFrame;
        }
        else
            topFrame1.enabled = false;

        if (centerReward.topFrame)
        {
            topFrame2.enabled = true;
            topFrame2.sprite = centerReward.topFrame;
        }
        else
            topFrame2.enabled = false;

        if (rightReward.topFrame)
        {
            topFrame3.enabled = true;
            topFrame3.sprite = rightReward.topFrame;
        }
        else
            topFrame1.enabled = false;
    }

    private RewardBase OpenBox(RandomReward rr)
    {
        int sumAppearance = 0;
        foreach (var r in rr.rewards)
        {
            sumAppearance += r.appearance;
        }

        int randomNum = Random.Range(0, sumAppearance);

        foreach (var r in rr.rewards)
        {
            randomNum -= r.appearance;
            if (randomNum <= 0)
            {
                return r.reward;
            }
        }

        Debug.Log(this);
        return null;
    }

    public void SelectReward(int num)
    {
        if (num == selectNum)
            selectNum = 0;
        else
            selectNum = num;

        emphasisFrame.SetActive(true);
        switch (selectNum)
        {
            case 0:
                emphasisFrame.SetActive(false);
                expanationText.text = string.Empty;
                break;
            case 1:
                emphasisFrame.transform.position = content1.transform.position;
                expanationText.text = leftReward.explanation;
                break;
            case 2:
                emphasisFrame.transform.position = content2.transform.position;
                expanationText.text = centerReward.explanation;
                break;
            case 3:
                emphasisFrame.transform.position = content3.transform.position;
                expanationText.text = rightReward.explanation;
                break;
            default:
                break;
        }
    }

    public void DisideReward()
    {
        RewardBase selectedReward = null;
        switch (selectNum)
        {
            case 0:
                return;
                //break;
            case 1:
                selectedReward = leftReward;
                break;
            case 2:
                selectedReward = centerReward;
                break;
            case 3:
                selectedReward = rightReward;
                break;
        }

        switch (selectedReward.type)
        {
            case RewardType.Module:
                ModuleCtrl.instance.modules.Add(selectedReward as Module);
                break;
            case RewardType.Expansion:
                AugmentCtrl.instance.augments.Add(selectedReward as Expansion);
                break;
            case RewardType.Blueprint:
                selectedReward.Active();
                break;
        }
    }

    public void SellReward()
    {
        BombNumCtrl.instance.bombNum++;
    }
}
