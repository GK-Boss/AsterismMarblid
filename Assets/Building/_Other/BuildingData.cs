using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BuildingData")]
public class BuildingData : ScriptableObject
{
    [field: SerializeField] public Sprite ghost { get; private set; }
    [field: SerializeField] public BuildingBase prefab { get; private set; }
    [Header("=== 基礎 ===")]
    public bool unLoaked;
    [field: SerializeField] public bool isStatic { get; private set; } = false;
    [field:SerializeField] public float defaultHp { get; private set; } = 1.0f;
    [field: SerializeField] public float defaultBuildTime { get; private set; } = 1.0f;
    [field: SerializeField] public BuildCost[] defaultBuildingCosts { get; private set; }
    [field: SerializeField] public int defaultSize { get; private set; } = 1;

    // 外部参照用
    [HideInInspector] public float currentHp => (int)(((defaultHp + hpUpConstByAugment) * hpUpRatioByModule + hpUpConstByModule) * hpUpRatioByAugment);
    [HideInInspector] public float currentBuildTime => defaultBuildTime;
    [HideInInspector] public BuildCost[] currentBuildingCosts => defaultBuildingCosts;
    [HideInInspector] public int currentBuildingSize => defaultSize;

    #region Module
    [Header("=== モジュール ===")]
    public List<Module> uniqueModule;
    public int defaultModuleSlot = 1;
    public List<Module> modules;
    // 外部参照用
    [HideInInspector] public int currentModuleSlot => defaultModuleSlot + slotUp;
    #endregion

    #region Inventory
    [Header("=== インベントリー ===")]
    public bool useInventory;
    [ShowIf(nameof(useInventory))] public int defaultCapasity;
    // 外部参照用
    [HideInInspector] public int currentCapasity => defaultCapasity;
    #endregion

    #region Turret
    [Header("=== タレット ===")]
    public bool useTurret;
    [ShowIf(nameof(useTurret))] public float defaultPower = 1;
    [ShowIf(nameof(useTurret)), Tooltip("X / s")] public float defaultRate = 1;
    [ShowIf(nameof(useTurret))] public BulletRecipe[] defaultBulletRecipes;
    [ShowIf(nameof(useTurret))] public float defaultAngleSpeed = 360;
    [ShowIf(nameof(useTurret))] public float defaultRange = 10;
    // 外部参照用
    [HideInInspector] public float currentPower;
    [HideInInspector] public float currentRate => defaultRate;
    [HideInInspector] public float currentAngleSpeed;
    [HideInInspector] public float currentRange => defaultRange;
    #endregion

    #region Mining
    [Header("=== マイニング ===")]
    public bool useMining;
    [ShowIf(nameof(useMining))] public float defaultMiningSpeed;
    // 外部参照用
    [HideInInspector] public float currentMiningSpeed;
    #endregion

    // 内部計算用

    // HP　((defaultHp + hpUpConstByAugment) * hpUpRatioByModule + hpUpConstByModule) * hpUpRatioByAugment
    [HideInInspector] public float hpUpConstByModule = 1;  // HP定数上昇(モジュール)
    [HideInInspector] public float hpUpRatioByModule = 1;  // HP倍率上昇(モジュール)
    [HideInInspector] public float hpUpConstByAugment = 1; // HP定数上昇(オーグメント)
    [HideInInspector] public float hpUpRatioByAugment = 1; // HP倍率上昇(オーグメント)

    [HideInInspector] public int slotUp; // モジュール容量上昇

    //private int capacityUp; // インベントリ容量上昇

    [HideInInspector] public float powerUp_Alpha; // 基礎攻撃力定数上昇
    [HideInInspector] public float powerUp_Beta;  // 攻撃力倍率
    [HideInInspector] public float powerUp_Theta; // 攻撃力定数上昇

    [HideInInspector] public float speedUpRatio;  // スピード倍率


    private void OnEnable()
    {
        hpUpConstByModule = 10;
        hpUpRatioByModule = 1.5f;
        hpUpConstByAugment = 10;
        hpUpRatioByAugment = 1.5f;
    }

    public void ClearModule()
    {
        //capacityUp = 0;
        powerUp_Alpha = 0;
        powerUp_Beta = 1;
        powerUp_Theta = 0;
        speedUpRatio = 1;
    }
    public void SetModule(Module module, bool isAugment = false)
    {
        // 内部計算用の数値を変えたい

        if (!isAugment)
            modules.Add(module);
    }
    public void ApplyModule()
    {
        //currentModuleSlot = defaultModuleSlot + slotUp;

        if (useInventory)
        {
            //currentCapasity = defaultCapasity + capacityUp;
        }

        if (useTurret)
        {
            currentPower = (defaultPower + powerUp_Alpha) * powerUp_Beta + powerUp_Theta;
        }

        if (useMining)
        {
            currentMiningSpeed = defaultMiningSpeed * speedUpRatio;
        }
    }
}

[System.Serializable]
public struct BuildCost
{
    public Resource resource;
    public float amount;
}

[System.Serializable]
public class BulletRecipe
{
    public bool active;
    public Resource resource;
    public GameObject bulletPrefab;
}