using UnityEngine;
using UnityEngine.UI;

public class BuildingInfomationPrefab : MonoBehaviour
{
    [SerializeField] Image resourceImage;
    [SerializeField] Text numText;

    public void Init(Resource resource, int num)
    {
        resourceImage.sprite = resource.prefab.GetComponent<SpriteRenderer>().sprite;
        numText.text = num.ToString();
    }
}
