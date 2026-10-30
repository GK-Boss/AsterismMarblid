using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUD1 : MonoBehaviour
{
    [SerializeField] private MenuButton menuButton;
    [SerializeField] private BagButton bagButton;
    [SerializeField] private GameObject pauseButon;
    [SerializeField] private Sprite[] pauseSprites;
    [SerializeField] private GameObject timeScaleButton;
    [SerializeField] private Sprite[] timeScaleSprites;
    [SerializeField] private GameObject skipButton;
    [HideInInspector] public bool pausing;

    [SerializeField] private Text riskLevelText;

    [SerializeField] private Text spawnerNumText;

    [SerializeField] private int interval;

    [SerializeField] private Text countDownText;
    private int countDown;

    void Start()
    {
        //NextWave();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuButton.SwitchMenu();
        }
        SpawnerNumText();
        RiskLevelText();
    }
    public void NextWave()
    {
        StopAllCoroutines();
        EnemySpawnerManager.instance.Spawn();

        countDown = interval;
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        int minutes;
        int seconds;
        while (countDown > 0)
        {
            minutes = countDown / 60;
            seconds = countDown % 60;
            if (minutes == 0)
                countDownText.text = "次のスポーン　" + seconds + "秒";
            else
                countDownText.text = "次のスポーン　" + minutes + "分" + seconds + "秒";
            countDown--;
            yield return new WaitForSeconds(1);
        }
        NextWave();
    }

    private void RiskLevelText()
    {
        riskLevelText.text = "危険度：" + EnemySpawnerManager.instance.GetRiskLevel();
    }
    public void SpawnerNumText()
    {
        spawnerNumText.text = "スポナー：" + EnemySpawnerManager.instance.GetSpawnerNum();
    }
}
