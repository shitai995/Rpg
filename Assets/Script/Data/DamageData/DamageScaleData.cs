// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-11 14:35:59
// 版本：V1.1
// 描述：伤害与元素效果倍率配置类，用于定义不同攻击/技能的伤害缩放系数和元素效果参数
// ========================================================

using System;
using UnityEngine;


[Serializable]
public class DamageScaleData 
{
    [Header("基础伤害倍率")]
    public float physical = 1;// 物理伤害倍率
    public float elemental = 1;// 元素伤害倍率

    [Header("冰冻")]
    public float chillDuration = 3;// 冰冻效果持续时间
    public float chillSlowMultiplier = .2f;// 冰冻减速倍率

    [Header("灼烧")]
    public float burnDuration = 3;// 灼烧效果持续时间
    public float burnDamageSacale = 1;// 灼烧伤害倍率

    [Header("感电")]
    public float shockDuration = 3;// 感电效果持续时间
    public float shockDamageScale = 1; // 感电伤害倍率
    public float shockCharge = .4f;// 感电充能系数
}
