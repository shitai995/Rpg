// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-13 22:19:54
// 版本：V1.1
// 描述：装备栏插槽数据结构，定义插槽类型与当前装备物品
// ========================================================

using System;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot
{
    [Tooltip("插槽类型（武器/护甲/饰品等）")]
    public ItemType slotType;

    [Tooltip("当前穿戴的装备")]
    public Inventory_Item equipedItem;

    /// <summary>
    /// 判断当前插槽是否有装备
    /// </summary>
    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;
}