// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 11:54:44
// 版本：V1.1
// 描述：所有道具的基础数据类（材料/消耗品/装备通用）
// ========================================================

using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    public string saveId { get; private set; }

    [Header("商店价格")]
    [Range(0, 10000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("掉落属性")]
    [Range(0, 1000)]
    public int itemRarity = 100;
    [Range(0, 100)]
    public float dropChance;
    [Range(0, 100)]
    public float maxDropChance = 65f;

    [Header("合成配方")]
    public Inventory_Item[] craftRecipe;

    [Header("物品基础信息")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("物品效果")]
    public ItemEffect_DataSO itemEffect;

    private void OnValidate()
    {
        dropChance = GetDropChance();

#if UNITY_EDITOR
        string path = UnityEditor.AssetDatabase.GetAssetPath(this);
        saveId = UnityEditor.AssetDatabase.AssetPathToGUID(path);
#endif
    }

    // 根据稀有度自动计算掉落概率
    public float GetDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxDropChance);
    }
}