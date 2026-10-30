using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//右のパネル
public class RewardBoxPanel : MonoBehaviour
{
    static public RewardBoxPanel instance;

    [SerializeField] private RewardPanel panel;
    [SerializeField] private RewardBoxView viewprefab;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject emptyText;
    [SerializeField] private GameObject emphasisFrame;

    private List<RewardBox> rewards = new();

    private RewardBox selectBox;
    private GameObject emphasisBox;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (emphasisBox)
        {
            emphasisFrame.SetActive(true);
            emphasisFrame.transform.position = new(transform.position.x, emphasisBox.transform.position.y);
        }
        else
        {
            emphasisFrame.SetActive(false);
        }
    }

    #region 表示
    private void OnEnable()
    {
        Clear();

        if (RewardManager.instance)
            rewards = RewardManager.instance.GetRewardBoxList();
        if (rewards.Count > 0)
        {
            emptyText.SetActive(false);

            StopAllCoroutines();
            StartCoroutine(GenerateCoroutine());
        }
        else
        {
            emptyText.SetActive(true);
        }
    }
    private void Clear()
    {
        selectBox = null;
        emphasisBox = null;
        emphasisFrame.SetActive(false);

        for (int i = content.childCount - 1;i >= 0;i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
    private IEnumerator GenerateCoroutine()
    {
        int generatedCount = 0;
        while (rewards.Count > generatedCount)
        {
            if (rewards[generatedCount] == null)
            {
                Debug.Log("Over");
                yield break;
            }
            RewardBoxView view = Instantiate(viewprefab, content);
            view.icon.sprite = rewards[generatedCount].boxIcon;
            view.text.text = rewards[generatedCount].byName;
            view.box = rewards[generatedCount];

            generatedCount++;
            yield return null;
        }
    }
    #endregion

    #region 選択
    public void SelectRewardBox(RewardBox rewardBox, GameObject _emphasisBox)
    {
        if (_emphasisBox == emphasisBox && rewardBox == selectBox)
        {
            selectBox = null;
            emphasisBox = null;
        }
        else
        {
            selectBox = rewardBox;
            emphasisBox = _emphasisBox;
        }
    }
    #endregion

    public void OpenBox()
    {
        if (!selectBox) return;
        Menu.Menu.instance.OpenRewardPanel();
        panel.SetBox(selectBox);
        RewardManager.instance.RemoveReward(selectBox);
        OnEnable();
    }

    public void SellBox()
    {
        BombNumCtrl.instance.bombNum += 2;
        RewardManager.instance.RemoveReward(selectBox);
        OnEnable();
    }
}
