using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationScreen : MonoBehaviour
{
    public void Yes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void No()
    {
        gameObject.SetActive(false);
    }
}
