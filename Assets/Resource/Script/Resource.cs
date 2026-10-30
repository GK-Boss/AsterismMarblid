using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Resource")]
public class Resource : ScriptableObject
{
    public string resourceName;

    public ResourcePrefab prefab;
}
