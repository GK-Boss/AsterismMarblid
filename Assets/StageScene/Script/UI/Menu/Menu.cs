using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        static public Menu instance;

        [Header("=== 各パネル ===")]
        [SerializeField] private GameObject playerPanel;
        [SerializeField] private GameObject buildingPanel;
        [SerializeField] private GameObject modulePanel;
        [SerializeField] private GameObject settingPanel;
        [Header("報酬パネル")]
        [SerializeField] private GameObject rewardPanel;

        [Header("=== 各ボタンテキスト ===")]
        [SerializeField] private Text playerButtonText;
        [SerializeField] private Text buildingButtonText;
        [SerializeField] private Text moduleButtonText;
        [SerializeField] private Text settingButtonText;

        private void Awake()
        {
            instance = this;
        }
        public void OpenMenu(int panelNum = 0)
        {
            gameObject.SetActive(true);

            switch (panelNum)
            {
                case 0:
                    break;
                case 1:
                    OpenPlayerPanel();
                    break;
                case 2:
                    OpenBuildingPanel();
                    break;
                case 3:
                    OpenModulePanel();
                    break;
                case 4:
                    OpenSettingPanel();
                    break;
                default:
                    break;
            }
        }

        public void CloseMenu()
        {
            CloseAllPanel();
            gameObject.SetActive(false);
        }

        public void CloseAllPanel()
        {
            rewardPanel.SetActive(false);
            playerPanel.SetActive(false);
            buildingPanel.SetActive(false);
            modulePanel.SetActive(false);
            settingPanel.SetActive(false);

            playerButtonText.color = Color.white;
            buildingButtonText.color = Color.white;
            moduleButtonText.color = Color.white;
            settingButtonText.color = Color.white;
        }

        public void OpenRewardPanel()
        {
            bool same = !rewardPanel.activeInHierarchy;
            CloseAllPanel();
            if (same)
                rewardPanel.SetActive(true);
        }
        public void OpenPlayerPanel()
        {
            bool same = !playerPanel.activeInHierarchy;
            CloseAllPanel();
            if (same)
            {
                playerPanel.SetActive(true);
                playerButtonText.color = Color.yellow;
            }
        }

        public void OpenBuildingPanel()
        {
            bool same = !buildingPanel.activeInHierarchy;
            CloseAllPanel();
            if (same)
            {
                buildingPanel.SetActive(true);
                buildingButtonText.color = Color.yellow;
            }
        }

        public void OpenModulePanel()
        {
            bool same = !modulePanel.activeInHierarchy;
            CloseAllPanel();
            if (same)
            {
                modulePanel.SetActive(true);
                moduleButtonText.color = Color.yellow;
            }
        }

        public void OpenSettingPanel()
        {
            bool same = !settingPanel.activeInHierarchy;
            CloseAllPanel();
            if (same)
            {
                settingPanel.SetActive(true);
                settingButtonText.color = Color.yellow;
            }
        }

        public void ExitGame()
        {
            SceneManager.LoadScene("StageSelect");
        }

    }
}
