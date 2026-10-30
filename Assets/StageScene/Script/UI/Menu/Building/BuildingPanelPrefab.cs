using UnityEngine;
using UnityEngine.UI;

namespace Menu.Building
{
    public class BuildingPanelPrefab : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text nameText;

        private BuildingData data;
        public void Setting(BuildingData _data)
        {
            data = _data;
            if (data.unLoaked)
            {
                icon.sprite = data.ghost;
                nameText.text = data.name;
            }
            else
            {
                icon.sprite = null;
                nameText.text = "No Blueprint...";
            }
        }

        public void OnClick()
        {
            BuildingPanel.instance.Select(data);
        }
    }
}
