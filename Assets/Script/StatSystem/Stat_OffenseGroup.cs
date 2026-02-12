// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 16:39:00
// 版本：V1.1
// 描述：
// ========================================================
using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;
    // 物理伤害
    public Stat damage;          // 基础物理伤害
    public Stat critPower;       // 暴击威力
    public Stat critChance;      // 暴击概率
    public Stat armorReduction;  // 破甲值

    // 元素伤害
    public Stat fireDamage;      // 火焰伤害
    public Stat iceDamage;       // 冰霜伤害
    public Stat lightningDamage; // 雷电伤害

}
