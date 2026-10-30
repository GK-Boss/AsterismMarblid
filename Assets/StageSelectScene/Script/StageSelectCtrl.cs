using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelectCtrl : MonoBehaviour
{
    [Header("説明文")]
    [SerializeField] private StageInfomation stageInfomation;
    [Header("スロット(5個)")]
    public Transform[] slots; // 0:FarLeft, 1:Left, 2:Center, 3:Right, 4:FarRight
    public GameObject[] asteroids;
    private SpriteRenderer[] srs;

    [Header("設定")]
    public float lerpSpeed = 10f; // 離した後の吸着スピード

    private float scrollPos = 0;   // 現在のスクロール位置（0 = ステージ0が中央）
    private float targetPos = 0;   // 最終的な目標位置
    private bool isDragging = false;
    private Vector3 lastMouseWorldPos;

    private string nextSceneName;

    void Start()
    {
        srs = new SpriteRenderer[asteroids.Length];
        for (int i = 0; i < asteroids.Length; i++)
            srs[i] = asteroids[i].GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        UpdatePositions();
    }

    void HandleInput()
    {
        // マウスのワールド座標を取得（Zはカメラとの距離を考慮）
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // カメラからの距離に合わせて調整
        Vector3 currentMouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) && !isDragging)
        {
            isDragging = true;
            lastMouseWorldPos = currentMouseWorldPos;
        }
        else if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) && isDragging)
        {
            // マウスが動いた「ワールド座標の差分」を計算
            float deltaX = currentMouseWorldPos.x - lastMouseWorldPos.x;

            // スロット間の距離（中央と右の間隔など）で割って、スクロール量に変換
            float slotDistance = Vector3.Distance(slots[2].position, slots[3].position);

            // マウスと同じスピードで動かす（左にドラッグしたら右へスクロール）
            scrollPos -= deltaX / slotDistance;

            // 範囲制限（0番目〜最後番目）
            scrollPos = Mathf.Clamp(scrollPos, 0, asteroids.Length - 1);
            targetPos = scrollPos;

            lastMouseWorldPos = currentMouseWorldPos;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            isDragging = false;
            // 離した瞬間に、一番近い整数のステージ番号を目標にする
            targetPos = Mathf.Round(scrollPos);
        }

        // ドラッグしていない時は、目標地点（整数）へ滑らかに移動
        if (!isDragging)
        {
            scrollPos = Mathf.Lerp(scrollPos, targetPos, Time.deltaTime * lerpSpeed);
        }
    }

    void UpdatePositions()
    {
        for (int i = 0; i < asteroids.Length; i++)
        {
            // 各ステージが「中央スロット(Index 2)」から見てどこにいるべきか
            // 例：scrollPosが0のとき、i=0のステージは offset=0 (中央)
            float offset = i - scrollPos;

            // スロットのインデックスに変換 (中央が2なので、2を足す)
            float slotIndex = offset + 2;

            // 5つのスロットの範囲内(0~4)であれば見た目を更新
            if (slotIndex > -1 && slotIndex < 5)
            {
                if (Mathf.RoundToInt(slotIndex) == 2)
                {
                    stageInfomation.ChangeText(asteroids[i], slotIndex - 2);
                    nextSceneName = asteroids[i].GetComponent<Asteroid>().sceneName;
                }

                asteroids[i].SetActive(true);
                ApplyVisuals(i, slotIndex);
            }
            else
            {
                // 画面外（スロット範囲外）なら非表示
                asteroids[i].SetActive(false);
            }


        }
    }

    void ApplyVisuals(int asteroidIdx, float slotIndex)
    {
        // 小数点のslotIndexを使って、隣り合うスロット間を補間する
        int lower = Mathf.FloorToInt(slotIndex);
        int upper = Mathf.CeilToInt(slotIndex);
        float t = slotIndex - lower;

        // 範囲外エラー防止
        lower = Mathf.Clamp(lower, 0, 4);
        upper = Mathf.Clamp(upper, 0, 4);

        // 座標とスケールの補間
        asteroids[asteroidIdx].transform.position = Vector3.Lerp(slots[lower].position, slots[upper].position, t);
        asteroids[asteroidIdx].transform.localScale = Vector3.Lerp(slots[lower].localScale, slots[upper].localScale, t);

        // 明るさの計算（中央のスロットIndex 2 に近いほど明るい）
        // 中央(2)からの距離 0 = 明るさ 1.0 / 距離 2以上 = 明るさ 0.3 など
        float distFromCenter = Mathf.Abs(slotIndex - 2);
        float brightness = Mathf.Max(0.3f, 1.0f - (distFromCenter * 0.35f));
        srs[asteroidIdx].color = new Color(brightness, brightness, brightness, 1);
    }

    public void EnterStage()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
