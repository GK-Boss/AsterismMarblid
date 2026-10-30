using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    [Header("3, 2, 1")]
    [SerializeField] private Sprite[] sprites;
    [Header("テキスト")]
    [SerializeField] private Text countdownText;

    [Header("爆発範囲")]
    public int explosionScale;

    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        sr.sprite = sprites[0];
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        sr.sprite = sprites[1];
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        sr.sprite = sprites[2];
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        Explosion();
    }

    private void Explosion()
    {
        List<Vector3Int> outerPosList = new()
        {
            Vector3Int.RoundToInt(transform.position) 
        };
        List<Vector3Int> willAddPosInt = new();

        Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
        for (int i = 0;i < explosionScale; i++)
        {
            foreach(Vector3Int posInt in outerPosList)
            {
                if (!MapManager.instance.HasTile(posInt) || MapManager.instance.GetTile(posInt).tileBase.name == "Tile_Wall")
                {
                    Tile stoneTile = TileDictionary.instance.GetTile("Tile_Stone");
                    MapManager.instance.ChangeTile(posInt, stoneTile);
                    MapManager.instance.RegisterTile(posInt, stoneTile);
                }
                foreach (Vector3Int dir in directions)
                {
                    Vector3Int currenPosInt = posInt + dir;
                    if (!outerPosList.Contains(currenPosInt) && !willAddPosInt.Contains(currenPosInt))
                    {
                        willAddPosInt.Add(currenPosInt);
                    }
                }
            }
            outerPosList.AddRange(willAddPosInt);

            if (i == explosionScale - 1)
            {
                foreach (Vector3Int posInt in outerPosList)
                {
                    if (!MapManager.instance.HasTile(posInt))
                    {
                        MapManager.instance.GenerateBiome(posInt);
                        break;
                    }
                }
            }
            willAddPosInt.Clear();
        }

        Destroy(gameObject);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
