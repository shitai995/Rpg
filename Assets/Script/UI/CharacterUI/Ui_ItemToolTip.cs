// ========================================================
// 作者：娇娇
// 版本：V1.1
// 描述：物品提示框UI，显示道具名称、类型、属性、效果描述
// ========================================================

using System.Text;
using TMPro;
using UnityEngine;

public class Ui_ItemToolTip : UI_ToolTip
{
    [Header("提示框文本组件")]
    [SerializeField] private TextMeshProUGUI itemName;   // 道具名称
    [SerializeField] private TextMeshProUGUI itemType;   // 道具类型
    [SerializeField] private TextMeshProUGUI itemInfo;    // 道具描述/属性

    /// <summary>
    /// 显示/隐藏物品提示框，并填充道具信息
    /// </summary>
    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRect);

        // 无物品时直接返回
        if (itemToShow == null)
            return;

        // 填充基础信息
        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    /// <summary>
    /// 根据道具类型生成对应的描述文本
    /// </summary>
    public string GetItemInfo(Inventory_Item item)
    {
        // 材料类型：固定描述
        if (item.itemData.itemType == ItemType.Material)
            return "Used for crafting.";

        // 消耗品：显示效果描述
        if (item.itemData.itemType == ItemType.Consumable)
            return item.itemData.itemEffect.effectDescription;

        // 装备类型：拼接属性加成 + 独特效果
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        // 遍历装备属性修饰器
        foreach (var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            // 判断属性是否显示百分比
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+ " + modValue + " " + modType);
        }

        // 存在独特效果则追加显示
        if (item.itemEffect != null)
        {
            sb.AppendLine();
            sb.AppendLine("Unique effect:");
            sb.AppendLine(item.itemEffect.effectDescription);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 将属性枚举转换为显示名称
    /// </summary>
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

    /// <summary>
    /// 判断属性是否需要显示百分比符号
    /// </summary>
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