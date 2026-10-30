using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Menu.Building
{
    public class BuildingPanel : MonoBehaviour
    {
        static public BuildingPanel instance;

        [SerializeField] private List<BuildingData> datas;

        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject viewPanel;
        [SerializeField] private BuildingDetailPanel detailPanel;

        private List<BuildingPanelPrefab> generated = new();

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            viewPanel.SetActive(true);
            detailPanel.gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine(CheckUnlocked());
        }
        public IEnumerator CheckUnlocked()
        {
            if (generated.Count != datas.Count)
            {
                generated.Clear();
                for (int i = content.childCount - 1; i >= 0; i--)
                {
                    Destroy(content.GetChild(i).gameObject);
                }

                foreach (var data in datas)
                {
                    generated.Add(Instantiate(prefab, content).GetComponent<BuildingPanelPrefab>());
                }
            }

            for (int i = 0; i < datas.Count; i++)
            {
                generated[i].Setting(datas[i]);
                yield return null;
            }
        }

        public void Select(BuildingData data)
        {
            viewPanel.SetActive(false);
            detailPanel.gameObject.SetActive(true);
            detailPanel.SetData(data);
        }

        public void Back()
        {
            viewPanel.SetActive(true);
            detailPanel.gameObject.SetActive(false);
        }
    }
}
