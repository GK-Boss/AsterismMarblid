using UnityEngine;

public class BombNumCtrl : MonoBehaviour
{
    static public BombNumCtrl instance;

    public int bombNum;

    private void Awake()
    {
        instance = this;
    }
}
