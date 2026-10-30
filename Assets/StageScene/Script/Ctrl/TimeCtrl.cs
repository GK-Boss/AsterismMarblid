using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCtrl : MonoBehaviour
{
    static public TimeCtrl Instance { get; private set; }

    private float timeScale = 1.0f;

    private PauseButton pauseButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        pauseButton = FindFirstObjectByType<PauseButton>();
    }
    public void ChangeTimeScale(float _timeScale)
    {
        timeScale = _timeScale;
        if (Time.timeScale != 0)
            Time.timeScale = _timeScale;
    }
    public void TimeStop()
    {
        Time.timeScale = 0.0f;
    }
    public void TimeResume()
    {
        if (!pauseButton.isPausing)
            Time.timeScale = timeScale;
    }
}
