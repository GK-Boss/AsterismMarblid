using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    [SerializeField]
    private Dictionary<Resource, int> resources = new();

    void Awake()
    {
        instance = this;
    }

    public void Add(Resource res)
    {
        if (!resources.ContainsKey(res))
            resources[res] = 0;

        resources[res]++;
    }

    public int Get(Resource data)
    {
        return resources.ContainsKey(data) ? resources[data] : 0;
    }

    public bool CheckResourceNum(Resource res, int amount)
    {
        if (resources.ContainsKey(res))
            return resources[res] > amount;
        else
            return false;
    }
    public void ChangeResourceNum(Resource res, int amount)
    {
        if (resources.ContainsKey(res))
            resources[res] += amount;
        else
            Debug.Log("リソースエラー");
    }


}