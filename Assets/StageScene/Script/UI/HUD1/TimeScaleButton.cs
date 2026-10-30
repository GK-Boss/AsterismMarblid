using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleButton : MonoBehaviour
{
    [SerializeField] private Sprite[] sprits;

    private Image image;
    private int index;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void ChangeTimeScale()
    {
        index++;
        if (index >= 4)
        {
            index = 0;
        }
        image.sprite = sprits[index];
        TimeCtrl.Instance.ChangeTimeScale(index + 1);
    }
}
