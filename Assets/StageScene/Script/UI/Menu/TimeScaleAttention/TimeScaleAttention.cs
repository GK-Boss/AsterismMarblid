using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleAttention : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private GameObject stopMark;
    [SerializeField] private GameObject continueMark;
    private void Update()
    {
        if (Mathf.Round(Time.timeScale) == 0)
        {
            text.text = "ÉQÅ[ÉÄí‚é~íÜ...";
            text.color = Color.white;
            //stopMark.SetActive(true);
            //continueMark.SetActive(false);
        }
        else
        {
            text.text = "!Attention!\nÉQÅ[ÉÄêiçsíÜ...";
            text.color = Color.red;
            //stopMark.SetActive(false);
            //continueMark.SetActive(true);
        }
    }
}
