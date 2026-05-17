// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-15 18:25:03
// 版本：V1.1
// 描述：属性槽UI，显示单个属性数值与提示
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player_Stats playerStats;
    private RectTransform rect;
    private UI ui;

    [Header("属性配置")]
    [SerializeField] private StatType statSlotType;

    [Header("UI显示")]
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    // 编辑器自动重命名与设置标题
    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    // 鼠标悬浮：显示属性提示
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect, statSlotType);
    }

    // 鼠标离开：关闭提示
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }

    /// <summary>
    /// 更新当前属性槽的数值显示
    /// </summary>
    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);

        if (statToUpdate == null && statSlotType != StatType.ElementalDamage)
        {
            Debug.Log($"You do not have {statSlotType} implemented on the player!");
            return;
        }

        float value = 0;

        switch (statSlotType)
        {
            // 核心主属性
            case StatType.Strength:
                value = playerStats.major.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue();
                break;

            // 攻击属性
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritPower:
                value = playerStats.GetCritPower();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100;
                break;

            // 防御属性
            case StatType.MaxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case StatType.HealthRegen:
                value = playerStats.resources.healthRegen.GetValue();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;

            // 元素伤害
            case StatType.IceDamage:
                value = playerStats.offense.iceDamage.GetValue();
                break;
            case StatType.FireDamage:
                value = playerStats.offense.fireDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType element, 1);
                break;

            // 元素抗性
            case StatType.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice) * 100;
                break;
            case StatType.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire) * 100;
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning) * 100;
                break;
        }

        // 判断是否显示百分比
        statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
    }

    /// <summary>
    /// 判断属性是否为百分比类型
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

    /// <summary>
    /// 将属性枚举转为UI显示名称
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
            case StatType.ElementalDamage: return "Elemental Damage";

            case StatType.IceResistance: return "Ice Resistance";
            case StatType.FireResistance: return "Fire Resistance";
            case StatType.LightningResistance: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }
}