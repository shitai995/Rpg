// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 19:58:22
// 版本：V1.1
// 描述：Buff效果数据结构，存储属性类型与增加值
// ========================================================

using System;
using UnityEngine;

[Serializable]
public class BuffEffectData
{
    [Tooltip("要修改的属性类型")]
    public StatType type;

    [Tooltip("属性修改数值（正负均可）")]
    public float value;
}