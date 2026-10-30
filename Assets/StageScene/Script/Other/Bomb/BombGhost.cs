using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class BombGhost : MonoBehaviour
{
    [SerializeField] private GameObject highlightObj;
    [SerializeField] private Bomb bomb;
    [HideInInspector] public SpriteRenderer sr;
    private List<Vector3Int> highlightAreas = new();

    int pastScale = 0;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (MapManager.instance.HasTile(Vector3Int.RoundToInt(transform.position)) && BombNumCtrl.instance.bombNum > 0)
        {
            sr.color = Color.white;
            if (bomb.explosionScale != pastScale)
                GenerateHighlightArea(bomb.explosionScale);
        }
        else
        {
            sr.color = Color.red;
            GenerateHighlightArea(0);
        }
    }

    private void GenerateHighlightArea(int scale)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        highlightAreas.Clear();
        for (int i = 0; i < scale; i++)
        {
            List<Vector3Int> changePosList = new();
            for (int x = -i; x <= i; ++x)
            {
                int y = i - Mathf.Abs(x);

                if (!changePosList.Contains(new(x, y)))
                    changePosList.Add(new(x, y));
                if (!changePosList.Contains(new(x, -y)))
                    changePosList.Add(new(x, -y));
            }
            highlightAreas.AddRange(changePosList);
        }

        foreach (Vector3Int posInt in highlightAreas)
        {
            Instantiate(highlightObj, transform.position + posInt, Quaternion.identity, transform);
        }
        pastScale = scale;
    }
}
