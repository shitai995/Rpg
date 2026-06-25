// ========================================================
// 作者：娇娇 
// 创建时间：2026-02-05 18:24:04
// 版本：V1.1
// 描述：属性类型枚举（对应Stat_SetupSO中的所有可配置属性，用于属性修改/查询）
// 用途：统一属性标识，方便代码中动态修改、获取指定属性值
// ========================================================

using UnityEngine;

/// <summary>
/// 属性类型枚举
/// 与Stat_SetupSO中的属性一一对应，用于属性系统的统一标识与操作
/// </summary>
public enum StatType
{
    // 基础资源属性
    MaxHealth,               // 最大生命值
    HealthRegen,             // 生命回复速度（每秒）

    // 核心主属性
    Strength,                // 力量（影响物理伤害、护甲）
    Agility,                 // 敏捷（影响攻击速度、闪避、暴击率）
    Intelligence,            // 智力（影响元素伤害、元素抗性）
    Vitality,                // 活力（影响最大生命值、生命回复）

    // 进攻-物理伤害属性
    AttackSpeed,             // 攻击速度（倍率，1为基础速度）
    Damage,                  // 基础物理伤害
    CritChance,              // 暴击概率（百分比，0-1）
    CritPower,               // 暴击倍率（百分比，150=1.5倍）
    ArmorReduction,          // 护甲穿透/减免值

    // 进攻-元素伤害属性
    FireDamage,              // 基础火焰伤害
    IceDamage,               // 基础冰霜伤害
    LightningDamage,         // 基础雷电伤害

    // 防御-物理伤害属性
    Armor,                   // 物理护甲（降低物理伤害）
    Evasion,                 // 闪避概率（百分比，0-1）

    // 防御-元素伤害属性
    IceResistance,           // 冰霜抗性（降低冰霜伤害/减速时长）
    FireResistance,          // 火焰抗性（降低火焰伤害/灼烧时长）
    LightningResistance,     // 雷电抗性（降低雷电伤害/充能速度）

    ElementalDamage          // 元素伤害
}