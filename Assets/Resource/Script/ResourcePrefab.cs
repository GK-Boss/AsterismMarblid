using UnityEngine;

public class ResourcePrefab : MonoBehaviour
{
    public Resource resource;

    [HideInInspector] public Direction fromDirection;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        sr.enabled = false;
    }

    public void ChangeVisivle(bool visivle)
    {
        sr.enabled = visivle;
    }
}
