using UnityEngine;
using UnityEngine.UI;

public class ModuleGhost : MonoBehaviour
{
    private Image image;
    [SerializeField] private Sprite defaultSprite;

    private RectTransform rt;

    private void Start()
    {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }
    public void Setting(Module module)
    {
        if (module)
            image.sprite = module.rewardIcon;
        else
            image.sprite = defaultSprite;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rt.position = new Vector3(pos.x, pos.y, 0);
    }
}
