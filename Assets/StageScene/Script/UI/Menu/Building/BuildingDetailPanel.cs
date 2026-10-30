using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Building
{
    public class BuildingDetailPanel : MonoBehaviour
    {
        [Header("Top")]
        [SerializeField] private Image image;
        [SerializeField] private Text buildingName;

        [Header("Button")]
        [SerializeField] private Text statusPanelButtonText;
        [SerializeField] private Text modulePanelButtonText;
        [SerializeField] private Text expansionPanelButtonText;
        [SerializeField] private GameObject expansionPanel;

        [Header("Status")]
        [SerializeField] private GameObject statusPanel;
        [SerializeField] private Text hpText;
        [SerializeField] private Text buildTimeText;
        [SerializeField] private Text sizeText;

        [Header("Module")]
        [SerializeField] private ModulePanel modulePanel;
        //[SerializeField] private EquippedPanel equippedPanel;
        //[SerializeField] private InventoryPanel inventoryPanel;
        //[SerializeField] private GameObject equipEmphasis;
        //[SerializeField] private GameObject inventoryEmphasis;

        [Header("CategorySetting")]
        [SerializeField] private GameObject attentionObj;
        [SerializeField] private GameObject buttonsObj;
        [SerializeField] private GameObject addButton;
        [SerializeField] private GameObject removeButton;
        [SerializeField] private GameObject moveRightButton;
        [SerializeField] private GameObject moveLeftButton;

        private BuildingData data;

        private void Update()
        {
            CheckOpenCategorySetting(BuildingCategories.instance.selectingCategory);
            CheckAddingOrRemoving(BuildingCategories.instance.ChackAdding(data));
        }
        public void SetData(BuildingData _data)
        {
            data = _data;

            image.sprite = data.ghost;
            buildingName.text = data.name;
            StartCoroutine(RefreshBuildingName());

            modulePanel.GenerateModuleSlot(data);
            //equippedPanel.GenerateModuleSlot(data.currentModuleSlot);
            //equippedPanel.SetModules(data.modules);
            //inventoryPanel.Generate();

            hpText.text = "HP:" + data.currentHp + "Å@Å@Å@[(" + data.defaultHp + " + " + data.hpUpConstByAugment + ") Å~ " + data.hpUpConstByModule + " + " + data.hpUpConstByModule + "] Å~ " + data.hpUpRatioByAugment;


            ChangeDetail(0);
        }

        private IEnumerator RefreshBuildingName()
        {
            yield return null;
            buildingName.text = data.name + " ";
        }

        public void ChangeDetail(int detailNum)
        {
            statusPanelButtonText.color = detailNum == 0 ? Color.yellow : Color.white;
            statusPanel.SetActive(detailNum == 0);

            modulePanelButtonText.color = detailNum == 1 ? Color.yellow : Color.white;
            modulePanel.gameObject.SetActive(detailNum == 1);

            expansionPanelButtonText.color = detailNum == 2 ? Color.yellow : Color.white;
            expansionPanel.SetActive(detailNum == 2);
        }

        public void ChangeBuildingCategoryIcon()
        {
            BuildingCategories.instance.ChangeCategoryIcon(data.ghost);
        }

        public void AddBuildingInCategory()
        {
            BuildingCategories.instance.AddBuilding(data);
        }
        public void RemoveBuildingInCategory()
        {
            BuildingCategories.instance.RemoveBuilding(data);
        }

        public void MoveRightInCategory()
        {
            BuildingCategories.instance.MoveRight(data);
        }
        public void MoveLeftInCategory()
        {
            BuildingCategories.instance.MoveLeft(data);
        }

        private void CheckOpenCategorySetting(bool selectingCategory)
        {
            attentionObj.SetActive(!selectingCategory);
            buttonsObj.SetActive(selectingCategory);
        }

        private void CheckAddingOrRemoving(bool adding)
        {
            addButton.SetActive(!adding);
            removeButton.SetActive(adding);

            moveRightButton.SetActive(adding);
            moveLeftButton.SetActive(adding);
        }
    }
}
