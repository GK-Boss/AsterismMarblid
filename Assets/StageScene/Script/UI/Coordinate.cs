using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coordinate : MonoBehaviour
{
    private Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        Vector3Int currentPosInt = Vector3Int.RoundToInt(Camera.main.transform.position);
        text.text = "(X : " + currentPosInt.x + ", Y : " + currentPosInt.y + ")";
    }
}
