using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private BuildingData data;
    public void Setting(BuildingData _data)
    {
        data = _data;
        GetComponent<Image>().sprite = data.ghost;
    }

    public void OnClick()
    {
        HUD2.instance.SelectBuilding(data);
        HUD2.instance.Emphasis(gameObject);
    }

    public BuildingData GetData()
    {
        return data;
    }
}
