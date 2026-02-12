// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 16:06:48
// 版本：V1.1
// 描述：属性系统核心类（支持基础值+多源修改器叠加计算最终属性值）
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Stat
{
    // 属性基础值（无任何加成/减益时的原始值）
    [SerializeField] private float baseValue;
    // 属性修改器列表（存储所有来源的加成/减益值，如装备、buff、技能）
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();
    // 是否需要重新计算最终值
    private bool needToCalculate = true;
    private float finalValue;
    public float GetValue()
    {
        if (needToCalculate)
        {
            finalValue = GetFinalValue();
            needToCalculate = false;
        }

        return finalValue;
    }
    /// <summary>
    /// 添加属性修改器（如装备加成、buff减益）
    /// </summary>
    public void AddModifier(float value,string source)
    {

        StatModifier modToAdd = new StatModifier(value,source);
        modifiers.Add(modToAdd);
        needToCalculate = true;
    }
    /// <summary>
    /// 根据来源移除属性修改器（精准移除指定来源的所有加成/减益）
    /// </summary>
    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifiers => modifiers.source == source);
        needToCalculate = true;
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.Value;
        }
        return finalValue;
    }

    public void SetBaseValue(float value) => baseValue = value;

}


[Serializable]
public class StatModifier
{
    public float Value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.Value = value;
        this.source = source;
    }
}
