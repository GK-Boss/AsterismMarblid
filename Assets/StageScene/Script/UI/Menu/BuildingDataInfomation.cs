using UnityEngine;
using UnityEngine.UI;

public class BuildingDataInfomation : MonoBehaviour
{
    [SerializeField] private BuildingBase prefab;
    [SerializeField] private bool unLocked;

    [SerializeField] private Image icon;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Text buildingName;

    private void Start()
    {
        Debug.Log("Start");

        if (unLocked)
        {
            icon.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            buildingName.text = prefab.name;
        }
        else
        {
            icon.sprite = defaultIcon;
            buildingName.text = "No Blueprint...";
        }
    }
    public void UnLock()
    {
        unLocked = true;
    }


}
