// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-13 21:31:34
// 版本：V1.1
// 描述：装备道具数据配置类
// ========================================================

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Equipment item", fileName = "Equipment data - ")]

public class EquipmentDataSo : ItemDataSo
{
    [Header("装备属性修饰")]
    [Tooltip("装备提供的属性加成列表")]
    public ItemModifier[] modifiers;
}

/// <summary>
/// 装备属性修饰器
/// </summary>
[Serializable]
public class ItemModifier
{
    [Tooltip("属性类型")]
    public StatType statType;

    [Tooltip("属性加成数值")]
    public float value;
}