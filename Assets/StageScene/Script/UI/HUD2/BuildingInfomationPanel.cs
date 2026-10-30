using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildingInfomationPanel : MonoBehaviour
{
    [SerializeField] private Image buildingImage;
    [SerializeField] private Text hpText;
    [SerializeField] private BuildingInfomationPrefab prefab;
    [SerializeField] private Transform content;

    private BuildingBase selectBuilding;
    // 生成したリソースUIだけを正確に記録するリスト
    private List<BuildingInfomationPrefab> generated = new();

    private void Update()
    {
        if (selectBuilding)
        {
            // アイコンとHPテキストの更新（contentの中にいても、直接参照しているので無事です）
            buildingImage.sprite = selectBuilding.data.ghost;
            hpText.text = "HP : " + selectBuilding.hp.ToString() + " / " + selectBuilding.data.currentHp;

            // 1. まずは「自分が生成したリソースUIだけ」をプールに戻して画面を綺麗にする
            ClearResourceUI();

            InventoryModule inventory = selectBuilding.GetComponent<InventoryModule>();
            if (inventory)
            {
                List<ResourcePrefab> resources = inventory.GetList();

                // インベントリの集計
                Dictionary<ResourcePrefab, int> dictionary = new();
                foreach (ResourcePrefab resource in resources)
                {
                    if (dictionary.ContainsKey(resource)) dictionary[resource]++;
                    else dictionary.Add(resource, 1);
                }

                // 2. 新たにプールから必要な数だけリソースUIを取得して配置
                foreach (var item in dictionary)
                {
                    BuildingInfomationPrefab bip = PoolManager.instance.Get<BuildingInfomationPrefab>(prefab.gameObject, content);

                    bip.transform.localScale = Vector3.one;
                    RectTransform rect = bip.GetComponent<RectTransform>();
                    if (rect != null) rect.anchoredPosition3D = Vector3.zero;

                    bip.Init(item.Key.resource, item.Value);

                    // ここでリストに登録しているため、他のUIと混ざりません
                    generated.Add(bip);
                }
            }
        }
        else
        {
            // 建物が選択されていないなら、リソース表示を消して初期化
            if (generated.Count > 0 || buildingImage.sprite != null)
            {
                ClearResourceUI();
                buildingImage.sprite = null;
                hpText.text = "HP : -- / --";
            }
        }
    }

    /// <summary>
    /// 自分が生成したリソースUIだけをピンポイントでプールに返却する
    /// </summary>
    private void ClearResourceUI()
    {
        // generatedに入っているリソースUIだけをループで処理
        foreach (var bip in generated)
        {
            if (bip != null)
            {
                // 自分（bip）のオブジェクトだけを非アクティブにする
                // これにより、同じContent内にいるアイコンやHPテキストは無傷で残ります
                PoolManager.instance.Release(bip);
                //bip.gameObject.SetActive(false);
            }
        }
        // リストを空にする
        generated.Clear();

        // 【変更点】
        // 以前のコードにあった「content.GetChild(i)をすべて非アクティブにするループ」は、
        // アイコンやHPテキストを消し去ってしまう原因になるため完全に削除しました！
    }

    public void SetInfomation(BuildingBase building)
    {
        selectBuilding = building;
    }
}