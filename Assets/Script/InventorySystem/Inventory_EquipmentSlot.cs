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
    public ItemType slotType;
    public Inventory_Item equipedItem;

    public Inventory_Item GetEquipedItem() => equipedItem;
    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;
}