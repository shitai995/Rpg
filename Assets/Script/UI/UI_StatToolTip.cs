// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 13:54:00
// 版本：V1.1
// 描述：属性提示框UI，显示属性详细说明
// ========================================================

using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 显示/隐藏属性提示框，并设置对应属性的说明文本
    /// </summary>
    public void ShowToolTip(bool show, RectTransform targetRect, StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statType);
    }

    /// <summary>
    /// 根据属性类型，返回对应的中文说明文本
    /// </summary>
    public string GetStatTextByType(StatType type)
    {
        switch (type)
        {
            // 核心主属性
            case StatType.Strength:
                return "每1点力量：\n 物理伤害 +1\n 暴击伤害 +0.5%";

            case StatType.Agility:
                return "每1点敏捷：\n 暴击率 +0.3%\n 闪避率 +0.5%";

            case StatType.Intelligence:
                return "每1点智力：\n 所有元素抗性 +0.5%\n 元素伤害 +1\n 若无任何元素伤害，则不生效";

            case StatType.Vitality:
                return "每1点活力：\n 最大生命值 +5\n 护甲 +1";

            // 物理伤害属性
            case StatType.Damage:
                return "决定你的物理攻击伤害";

            case StatType.CritChance:
                return "攻击造成暴击的概率";

            case StatType.CritPower:
                return "提升暴击造成的伤害";

            case StatType.ArmorReduction:
                return "攻击时无视目标护甲的百分比";

            case StatType.AttackSpeed:
                return "决定你的攻击速度";

            // 防御属性
            case StatType.MaxHealth:
                return "决定你的总生命值上限";

            case StatType.HealthRegen:
                return "每秒恢复的生命值";

            case StatType.Armor:
                return "降低受到的物理伤害\n 减伤上限 85%\n 当前减伤：" + playerStats.GetArmorMitigation(0) * 100 + "%";

            case StatType.Evasion:
                return "完全躲避攻击的概率\n 上限 85%";

            // 元素伤害
            case StatType.IceDamage:
                return "决定你的冰霜攻击伤害";

            case StatType.FireDamage:
                return "决定你的火焰攻击伤害";

            case StatType.LightningDamage:
                return "决定你的雷电攻击伤害";

            case StatType.ElementalDamage:
                return "元素伤害 = 三系元素总和\n 最高元素类型 = 全额伤害 + 对应效果\n 其余两系 = 50% 额外伤害";

            // 元素抗性
            case StatType.IceResistance:
                return "降低受到的冰霜伤害";

            case StatType.FireResistance:
                return "降低受到的火焰伤害";

            case StatType.LightningResistance:
                return "降低受到的雷电伤害";

            default:
                return "该属性暂无说明";
        }
    }
}