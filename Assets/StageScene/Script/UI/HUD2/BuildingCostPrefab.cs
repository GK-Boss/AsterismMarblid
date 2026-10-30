using UnityEngine;
using UnityEngine.UI;

public class BuildingCostPrefab : MonoBehaviour
{
    [SerializeField] private Image resourceImage;
    [SerializeField] private Text numText;

    private BuildCost cost;
    public void Init(BuildCost _cost)
    {
        cost = _cost;
        resourceImage.sprite = cost.resource.prefab.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        int num = ResourceManager.instance.Get(cost.resource);
        numText.text = cost.amount.ToString() + " / " + num;

        if (cost.amount > num)
        {
            numText.color = Color.red;
        }
        else
            numText.color = Color.white;
    }
}
