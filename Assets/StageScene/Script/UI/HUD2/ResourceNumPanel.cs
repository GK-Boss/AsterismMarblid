using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNumPanel : MonoBehaviour
{
    [SerializeField] private ResourceNum[] resourceNum;

    //[SerializeField] private Resource soilData;
    //[SerializeField] private Text soilNum;
    //[SerializeField] private Resource stoneData;
    //[SerializeField] private Text stoneNum;
    //[SerializeField] private Resource woodData;
    //[SerializeField] private Text woodNum;
    //[SerializeField] private Resource cooperData;
    //[SerializeField] private Text cooperNum;
    //[SerializeField] private Resource ironData;
    //[SerializeField] private Text ironNum;

    private void Update()
    {
        foreach (var rm in resourceNum)
        {
            Change(rm);
        }
    }

    private void Change(ResourceNum rm)
    {
        int num = ResourceManager.instance.Get(rm.resource);
        rm.numText.transform.parent.gameObject.SetActive(num != 0);
        rm.numText.text = num.ToString();
    }
}

[System.Serializable]
public struct ResourceNum
{
    [SerializeField] private string name;
    public Resource resource;
    public Text numText;
}
