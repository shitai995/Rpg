// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-13 22:03:21
// 版本：V1.1
// 描述：玩家背包与装备系统核心类
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    [Tooltip("装备插槽列表（武器/护甲/饰品等）")]
    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    /// <summary>
    /// 尝试穿戴装备：有空位直接穿，无空位替换第一个
    /// </summary>
    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        // 找到对应类型的装备槽
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);

        // 优先穿戴到空槽
        foreach (var slot in matchingSlots)
        {
            if (!slot.HasItem())
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        // 无空槽则替换第一个槽位
        var slotToReplace = matchingSlots[0];
        var itemToUneqip = slotToReplace.equipedItem;

        UnequipItem(itemToUneqip, true);
        EquipItem(inventoryItem, slotToReplace);
    }

    /// <summary>
    /// 穿戴装备：添加属性、绑定效果、从背包移除
    /// </summary>
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        // 保存当前血量百分比，防止因最大血量变化导致血量异常
        float savedHealthPercent = player.health.GetHealthPercent();

        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.stats);      // 添加装备属性
        slot.equipedItem.AddItemEffect(player);           // 绑定装备效果

        player.health.SetHealthToPercent(savedHealthPercent); // 恢复血量比例
        RemoveItem(itemToEquip);                          // 从背包移除
    }

    /// <summary>
    /// 卸下装备：移除属性、解绑效果、放回背包
    /// </summary>
    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        // 非替换情况下背包满则无法卸下
        if (!CanAddItem() && !replacingItem)
        {
            Debug.Log("No space!");
            return;
        }

        // 保存血量百分比
        float savedHealthPercent = player.health.GetHealthPercent();
        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if (slotToUnequip != null)
            slotToUnequip.equipedItem = null;             // 清空槽位

        itemToUnequip.RemoveModifiers(player.stats);      // 移除属性加成
        itemToUnequip.RemoveItemEffect();                 // 解绑装备效果

        player.health.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnequip);                           // 放回背包
    }
}