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
    // 物理伤害
    public Stat damage;
    public Stat critPower;// 暴击威力
    public Stat critChance;// 暴击概率
    public Stat armorReduction;

    // 元素伤害
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

}
