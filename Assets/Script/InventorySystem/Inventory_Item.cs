// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 20:32:11
// 版本：V1.1
// 描述：背包物品实例类（存储道具、堆叠、装备效果、属性修饰器）
// ========================================================

using System;
using System.Text;
using UnityEngine;

/// <summary>
/// 物品实例类：每个在背包/装备栏中的物品都是这个类的对象
/// </summary>
[Serializable]
public class Inventory_Item
{
    private string itemId; // 唯一ID，用于区分Buff/Modifier来源

    public ItemDataSO itemData;        // 物品基础数据
    public int stackSize = 1;          // 当前堆叠数量

    public ItemModifier[] modifiers;   // 装备属性加成
    public ItemEffect_DataSO itemEffect; // 物品被动/主动效果

    public int buyPrice { get; private set; }   // 购买价格
    public float sellPrice { get; private set; } // 出售价格

    // 构造函数：用物品数据创建实例
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * 0.35f; // 出售价为购买价的35%

        modifiers = EquipmentData()?.modifiers;
        itemId = itemData.itemName + " - " + Guid.NewGuid(); // 生成唯一ID
    }

    // 给玩家添加该装备的所有属性加成
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }

    // 移除该装备的所有属性加成
    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    // 订阅物品效果（装备时）
    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);
    // 取消物品效果（卸下时）
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();

    // 获取装备数据（如果是装备）
    private EquipmentDataSO EquipmentData()
    {
        if (itemData is EquipmentDataSO equipment)
            return equipment;
        return null;
    }

    // 堆叠判断
    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;

    // 生成物品提示信息（鼠标悬浮显示）
    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();

        // 材料类物品
        if (itemData.itemType == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Used for crafting");
            sb.AppendLine("");
            return sb.ToString();
        }

        // 消耗品类物品
        if (itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffect.effectDescription);
            sb.AppendLine("");
            return sb.ToString();
        }

        // 装备类物品：显示所有属性
        sb.AppendLine("");
        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value + "%" : mod.value.ToString();
            sb.AppendLine("+ " + modValue + " " + modType);
        }

        // 显示独特效果
        if (itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique effect:");
            sb.AppendLine(itemEffect.effectDescription);
        }

        sb.AppendLine("");
        return sb.ToString();
    }

    // 属性类型 → 显示名称
    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";

            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";

            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.CritChance: return "Critical Chance";
            case StatType.CritPower: return "Critical Power";
            case StatType.ArmorReduction: return "Armor Reduction";

            case StatType.FireDamage: return "Fire Damage";
            case StatType.IceDamage: return "Ice Damage";
            case StatType.LightningDamage: return "Lightning Damage";

            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }

    // 判断是否为百分比属性
    private bool IsPercentageStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
}