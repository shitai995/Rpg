// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-11 14:26:08
// 版本：V1.1
// 描述：元素效果数据类，用于存储计算后的元素异常状态（冰冻/灼烧/感电）最终生效参数
// ========================================================

using System;

[Serializable]
public class ElementalEffectData 
{
    public float chillDuration;// 冰冻效果持续时间
    public float chillSlowMultiplier;// 冰冻减速倍率

    public float burnDuration;// 灼烧效果持续时间
    public float totalBurnDamage;// 灼烧总伤害

    public float shockDuration;// 感电效果持续时间
    public float shockDamage;// 感电单次伤害值
    public float shockCharge;// 感电充能系数
    /// <summary>
    /// 构造函数：根据实体属性和伤害倍率配置，初始化元素效果的最终参数
    /// </summary>
    public ElementalEffectData(Entity_Stats entityStats,DamageScaleData damageScale)
    {
        // 初始化冰冻效果参数
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier = damageScale.chillSlowMultiplier;
        // 初始化灼烧效果参数
        burnDuration = damageScale.burnDuration;
        totalBurnDamage = entityStats.offense.fireDamage.GetValue() * damageScale.burnDamageSacale;
        // 初始化感电效果参数
        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offense.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;
    }
}
