// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:30:18
// 版本：V1.1
// 描述：角色基础属性配置SO，统一设置生命、攻防、元素属性等初始数值
// ========================================================

using UnityEngine;

/// <summary>
/// 角色初始属性配置文件
/// </summary>
[CreateAssetMenu(menuName = "RPG Setup/Default Stat Setup", fileName = "Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("资源属性")]
    public float maxHealth = 100;        // 最大生命值
    public float healthRegen;            // 生命回复

    [Header("物理攻击属性")]
    public float attackSpeed = 1;        // 攻击速度
    public float damage = 10;            // 基础伤害
    public float critChance;             // 暴击几率
    public float critPower = 150;        // 暴击倍率
    public float armorReduction;         // 护甲穿透

    [Header("元素攻击属性")]
    public float fireDamage;             // 火焰伤害
    public float iceDamage;              // 冰霜伤害
    public float lightningDamage;        // 雷电伤害

    [Header("物理防御属性")]
    public float armor;                   // 护甲值
    public float evasion;                 // 闪避率

    [Header("元素防御属性")]
    public float fireResistance;         // 火焰抗性
    public float iceResistance;          // 冰霜抗性
    public float lightningResistance;    // 雷电抗性

    [Header("主属性")]
    public float strength;                // 力量
    public float agility;                 // 敏捷
    public float intelligence;            // 智力
    public float vitality;                // 体力
}