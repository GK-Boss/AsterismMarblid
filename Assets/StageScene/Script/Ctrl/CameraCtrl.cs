using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CameraCtrl : MonoBehaviour
{
    static public CameraCtrl instance;
    
    [Header("ÉJÉÅÉâÇÃê›íË")]
    [SerializeField] private Tilemap tile;
    [Header("ÉJÉÅÉâÇÃê›íË")]
    [SerializeField] private float zoomSize = 1;
    [SerializeField] private float zoomSpeed = 10;
    [SerializeField] private float maxSize = 20;
    [SerializeField] private float minSize = 3;

    [HideInInspector] public bool stop;

    private Camera cam;
    private float targetSize;
    private Vector3 pastPos;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
    }

    void Update()
    {
        if (stop) return;

        float moveZ = CheckUI() ? 0 : Input.mouseScrollDelta.y * zoomSize;
        if ((Mathf.Sign(moveZ) == 1 && targetSize != minSize) || (Mathf.Sign(moveZ) == -1 && targetSize != maxSize))
        {
            targetSize -= moveZ;

            if (targetSize <= minSize)
                targetSize = minSize;
            else if (targetSize > maxSize)
                targetSize = maxSize;
        }

        if (Mathf.Abs(cam.orthographicSize - targetSize) < 0.05f)
            cam.orthographicSize = targetSize;
        else if (cam.orthographicSize != targetSize)
            cam.orthographicSize += (targetSize - cam.orthographicSize) * Time.unscaledDeltaTime * zoomSpeed;

        if (Input.GetKey(KeyCode.Mouse1))
            cam.transform.position += pastPos - cam.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 vec = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow))
                vec += Vector3.right;
            if (Input.GetKey(KeyCode.LeftArrow))
                vec += Vector3.left;
            if (Input.GetKey(KeyCode.UpArrow))
                vec += Vector3.up;
            if (Input.GetKey(KeyCode.DownArrow))
                vec += Vector3.down;

            cam.transform.position += 5 * vec.normalized * Time.deltaTime;
        }
            pastPos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (tile)
        {
            if (cam.transform.position.x > tile.cellBounds.max.x)
                cam.transform.position = new Vector3(tile.cellBounds.max.x, cam.transform.position.y, cam.transform.position.z);
            else if (cam.transform.position.x < tile.cellBounds.min.x)
                cam.transform.position = new Vector3(tile.cellBounds.min.x, cam.transform.position.y, cam.transform.position.z);
            if (cam.transform.position.y > tile.cellBounds.max.y)
                cam.transform.position = new Vector3(cam.transform.position.x, tile.cellBounds.max.y, cam.transform.position.z);
            else if (cam.transform.position.y < tile.cellBounds.min.y)
                cam.transform.position = new Vector3(cam.transform.position.x, tile.cellBounds.min.y, cam.transform.position.z);
        }
        else
        {
            Debug.Log("CameraCtrlÇ…ÇƒÅATilemapÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        }
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
