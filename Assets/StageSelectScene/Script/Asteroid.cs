using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("表示するステージの名前")]
    public string stageNsme;
    [Header("説明文")]
    [TextArea]public string explanation;
    [Header("遷移するシーンの名前(正確に)")]
    public string sceneName;
}
