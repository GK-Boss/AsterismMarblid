using System.Collections.Generic;
using UnityEngine;

namespace Menu.Building
{
    public class ModulePanel : MonoBehaviour
    {
        static public ModulePanel instance;

        [SerializeField] private ModuleSlotPrefab moduleSlotPrefab;
        [SerializeField] private ModuleGhost moduleGhost;
        [SerializeField] private Transform equippedContent;
        [SerializeField] private Transform inventoryContent;

        private List<ModuleSlotPrefab> equippedModules = new();
        private List<ModuleSlotPrefab> inventoryModules = new();

        private ModuleSlotPrefab selecting;

        private void Awake()
        {
            instance = this;
        }
        private void Update()
        {
            if (ModuleSlotPrefab.dragging)
            {
                moduleGhost.gameObject.SetActive(true);
                moduleGhost.Setting(ModuleSlotPrefab.dragging.module);
            }
            else
                moduleGhost.gameObject.SetActive(false);
        }

        public void GenerateModuleSlot(BuildingData data)
        {
            //Equipped
            int d = data.currentModuleSlot - equippedModules.Count;

            if (d > 0)
            {
                for (int i = 0; i < d; i++)
                {
                    equippedModules.Add(Instantiate(moduleSlotPrefab, equippedContent));
                }
            }
            else if (d < 0)
            {
                int pastSlotNum = equippedModules.Count;
                for (int i = 0; i < Mathf.Abs(d); i++)
                {
                    Debug.Log(pastSlotNum - i - 1);
                    ModuleSlotPrefab msp = equippedModules[pastSlotNum - i - 1];
                    equippedModules.Remove(msp);
                    Destroy(msp.gameObject);
                }
            }

            for (int i = 0; i < equippedModules.Count; i++)
            {
                if (i < data.modules.Count)
                    equippedModules[i].Setting(data.modules[i]);
                else
                    equippedModules[i].Setting(null);
            }

            //Inventory
            d = ModuleCtrl.instance.modules.Count - inventoryModules.Count;

            if (d > 0)
            {
                for (int i = 0; i < d; i++)
                {
                    inventoryModules.Add(Instantiate(moduleSlotPrefab, inventoryContent));
                }
            }
            else if (d < 0)
            {
                int pastSlotNum = inventoryModules.Count;
                for (int i = 0; i < Mathf.Abs(d); i++)
                {
                    ModuleSlotPrefab msp = inventoryModules[pastSlotNum - i - 1];
                    inventoryModules.Remove(msp);
                    Destroy(msp.gameObject);
                }
            }

            for (int i = 0; i < inventoryModules.Count; i++)
            {
                inventoryModules[i].Setting(ModuleCtrl.instance.modules[i]);
            }
        }

        public void Select(ModuleSlotPrefab msp)
        {
            if (selecting == msp)
            {
                selecting = null;
            }
            else
            {
                selecting = msp;
            }
        }
    }
}
