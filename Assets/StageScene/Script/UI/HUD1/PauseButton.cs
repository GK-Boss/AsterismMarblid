using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Sprite pauseButton;
    [SerializeField] private Sprite resumeButton;

    private Image image;
    [HideInInspector] public bool isPausing;

    void Start()
    {
        image = GetComponent<Image>();
        isPausing = false;
    }

    public void SwichPausing()
    {
        if (isPausing)
        {
            isPausing = false;
            TimeCtrl.Instance.TimeResume();
            image.sprite = pauseButton;
        }
        else
        {
            isPausing = true;
            TimeCtrl.Instance.TimeStop();
            image.sprite = resumeButton;
        }
    }
}
