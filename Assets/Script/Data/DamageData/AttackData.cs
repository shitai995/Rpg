// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-11 19:12:50
// 版本：V1.1
// 描述：攻击数据模型，用于封装单次攻击的所有伤害和效果相关数据
// ========================================================

using System;
using UnityEngine;
[Serializable]
public class AttackData
{
    public float phyiscalDamage;// 物理伤害值
    public float elementalDamage;// 元素伤害值
    public bool isCrit;// 是否触发暴击
    public ElementType element;// 攻击的元素类型（如火、水、雷等）

    public ElementalEffectData effectData;// 元素效果数据（如灼烧、冻结、感电等效果的参数）

    /// <summary>
    /// 构造函数：根据实体属性和伤害倍率数据初始化攻击数据
    /// </summary>
    public AttackData(Entity_Stats entityStats,DamageScaleData scaleData)
    {
        // 从实体属性中计算物理伤害，并输出是否触发暴击
        phyiscalDamage = entityStats.GetPhyiscalDamage(out isCrit, scaleData.physical);
        // 从实体属性中计算元素伤害，并输出攻击的元素类型
        elementalDamage = entityStats.GetElementalDamage(out  element,scaleData.elemental);
        // 初始化元素效果数据
        effectData = new ElementalEffectData(entityStats, scaleData);
    }
}
