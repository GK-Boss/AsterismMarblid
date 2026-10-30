using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombMode : MonoBehaviour
{
    [SerializeField] private BombGhost bombGhost;
    [SerializeField] private Bomb bomb;

    private GameObject generatedBomb;

    void Update()
    {
        Vector3Int mousePosInt = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePosInt.z = 0;

        if (generatedBomb == null)
        {
            if (CheckUI())
            {
                mousePosInt.z = -10;
                MoveGhost(mousePosInt);
                return;
            }
            MoveGhost(mousePosInt);
            bombGhost.sr.enabled = true;

            if (Input.GetKeyDown(KeyCode.Mouse0) && MapManager.instance.HasTile(mousePosInt) && BombNumCtrl.instance.bombNum > 0)
                GenerateBomb(mousePosInt);
        }
        else
        {
            MoveGhost(Vector3Int.RoundToInt(generatedBomb.transform.position));
            if (CheckUI())
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && mousePosInt == Vector3Int.RoundToInt(generatedBomb.transform.position))
                RemoveBomb();
        }
    }

    private void MoveGhost(Vector3Int posInt)
    {
        bombGhost.transform.position = posInt;
    }

    private void GenerateBomb(Vector3Int posInt)
    {
        bombGhost.sr.enabled = false;
        generatedBomb = Instantiate(bomb.gameObject, posInt, Quaternion.identity);
        BombNumCtrl.instance.bombNum--;
    }
    private void RemoveBomb()
    {
        Destroy(generatedBomb);
        BombNumCtrl.instance.bombNum++;
    }
    private bool CheckUI()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
            return true;

        List<RaycastResult> raycastResults = new();
        if (!EventSystem.current) return false;

        PointerEventData pointerEventData = new(EventSystem.current) { position = Input.mousePosition };
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count == 0) return false;

        return raycastResults[0].gameObject.layer == 5;
    }
}
