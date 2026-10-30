using UnityEngine;

public class FirstTemplate : MonoBehaviour
{
    [SerializeField] private Template[] templates;
    private void Start()
    {
        foreach (var template in templates)
        {
            BuildingBase bb = Instantiate(template.building, template.posInt, Quaternion.identity);
            bb.ChangeDirection(template.direction);
            bb.SkipBuild();
        }

        Destroy(gameObject);
    }

    [System.Serializable]
    public class Template
    {
        public Vector3Int posInt;
        public BuildingBase building;
        public MoreDirection direction;
    }
}