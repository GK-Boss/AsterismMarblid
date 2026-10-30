using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfomation: MonoBehaviour
{
    [SerializeField] private Text stageNameText;
    [SerializeField] private Text explanationText;
    public void ChangeText(GameObject asteroidObj, float t)
    {
        Asteroid asteroid = asteroidObj.GetComponent<Asteroid>();
        stageNameText.text = asteroid.stageNsme;
        stageNameText.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), Mathf.Abs(t));
        explanationText.text = asteroid.explanation;
        explanationText.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), Mathf.Abs(t));
    }
}
