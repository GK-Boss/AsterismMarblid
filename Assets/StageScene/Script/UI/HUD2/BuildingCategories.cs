using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCategories : MonoBehaviour
{
    static public BuildingCategories instance;

    [Range(1, 5)] public int categoryNum = 1;
    [SerializeField] private List<BuildingCategory> categories;
    [SerializeField] private Image emphasisFrame;

    private int pastIndex = -1;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ActiveCategory();
    }

    private void ActiveCategory()
    {
        for (int i = 0; i < categories.Count; i++)
        {
            if (i <= categoryNum - 1)
            {
                categories[i].gameObject.SetActive(true);
            }
            else
            {
                categories[i].gameObject.SetActive(false);
            }

            categories[i].SetIndex(i);
        }
    }

    public void ChangeCategoryIcon(Sprite icon)
    {
        if (pastIndex == -1)
        {
            return;
        }

        categories[pastIndex].SetIcon(icon);
    }

    public void AddBuilding(BuildingData data)
    {
        if (pastIndex == -1)
        {
            return;
        }

        categories[pastIndex].AddBuilding(data);
    }

    public bool ChackAdding(BuildingData data)
    {
        if (pastIndex == -1)
        {
            return false;
        }

        return categories[pastIndex].Adding(data);
    }

    public void RemoveBuilding(BuildingData data)
    {
        if (pastIndex == -1)
        {
            return;
        }

        categories[pastIndex].RemoveBuilding(data);
    }

    public void MoveRight(BuildingData data)
    {
        if (pastIndex == -1)
        {
            return;
        }

        categories[pastIndex].MoveRight(data);
    }

    public void MoveLeft(BuildingData data)
    {
        if (pastIndex == -1)
        {
            return;
        }

        categories[pastIndex].MoveLeft(data);
    }

    public void SelectCategory(int index)
    {
        HUD2.instance.ChangeMode(Mode.Build, true);

        if (index == pastIndex)
        {
            emphasisFrame.enabled = false;
            index = -1;
        }
        else
        {
            emphasisFrame.enabled = true;
            emphasisFrame.transform.position = categories[index].transform.position;
        }

        for (int i = 0; i < categories.Count; i++)
            categories[i].ChangeActiveContent(i == index);

        pastIndex = index;
    }

    public bool selectingCategory => pastIndex != -1;

    public void NoneSelectCategory()
    {
        emphasisFrame.enabled = false;

        for (int i = 0; i < categories.Count; i++)
            categories[i].ChangeActiveContent(false);

        pastIndex = -1;
    }
}
