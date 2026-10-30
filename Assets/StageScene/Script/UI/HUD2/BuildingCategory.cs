using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildingCategory : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Transform content;
    [SerializeField] private BuildingSelectButton buildingSelectButtonPrefab;

    [SerializeField] private List<BuildingData> datas;

    private List<BuildingSelectButton> generated = new();

    private int index;
    //private Image image;
    private List<BuildingData> buildings = new();

    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
    }

    public void SetIndex(int _index)
    {
        index = _index;
    }
    private void Start()
    {
        GenerateBuildingSelectButtons();
    }
    public void AddBuilding(BuildingData _data)
    {
        if (_data == null) return;
        foreach (BuildingData data in datas)
            if (_data == data) return;

        datas.Add(_data);
        GenerateBuildingSelectButtons();
    }

    public bool Adding(BuildingData _data)
    {
        foreach (BuildingData data in datas)
            if (_data == data) return true;

        return false;
    }

    public void RemoveBuilding(BuildingData _data)
    {
        if (_data == null) return;
        foreach (BuildingData data in datas)
        {
            if (_data == data)
            {
                datas.Remove(data);
                GenerateBuildingSelectButtons();
                return;
            }
        }
    }

    public void MoveRight(BuildingData _data)
    {
        if (!_data || !datas.Contains(_data)) return;

        int currentIndex = datas.IndexOf(_data);
        if (currentIndex < datas.Count - 1)
        {
            (datas[currentIndex], datas[currentIndex + 1]) = (datas[currentIndex + 1], datas[currentIndex]);
            GenerateBuildingSelectButtons();
        }
    }

    public void MoveLeft(BuildingData _data)
    {
        if (!_data || !datas.Contains(_data)) return;

        int currentIndex = datas.IndexOf(_data);
        if (currentIndex > 0)
        {
            (datas[currentIndex], datas[currentIndex - 1]) = (datas[currentIndex - 1], datas[currentIndex]);
            GenerateBuildingSelectButtons();
        }
    }
    private void GenerateBuildingSelectButtons()
    {
        if (datas.Count == generated.Count)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (generated[i].GetData() != datas[i])
                {
                    generated[i].Setting(datas[i]);
                }
            }
        }
        else
        {
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);
            }

            generated.Clear();
            for (int i = 0; i < datas.Count; i++)
            {
                generated.Add(Instantiate(buildingSelectButtonPrefab.gameObject, content).GetComponent<BuildingSelectButton>());
                generated[i].Setting(datas[i]);
            }
        }
    }

    public void ChangeActiveContent(bool active)
    {
        contentPanel.SetActive(active);
    }

    public void OnClick()
    {
        BuildingCategories.instance.SelectCategory(index);
    }
}
