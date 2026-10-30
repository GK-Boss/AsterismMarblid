using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCtrl : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
