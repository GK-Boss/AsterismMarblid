using Menu.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModuleSlotPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    static public ModuleSlotPrefab dragging;

    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultSprite;

    //private RectTransform rt;

    [HideInInspector] public Module module;

    private void Awake()
    {
        //rt = GetComponent<RectTransform>();
    }

    public void Setting(Module _module)
    {
        module = _module;

        if (_module)
            image.sprite = _module.rewardIcon;
        else
            image.sprite = defaultSprite;
    }

    public void OnClick()
    {
        ModulePanel.instance.Select(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = this;
        //image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //rt.Translate(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = null;
        //image.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragging)
        {
            Module nextModule = dragging.module;
            dragging.Setting(module);
            Setting(nextModule);
        }
    }
}
