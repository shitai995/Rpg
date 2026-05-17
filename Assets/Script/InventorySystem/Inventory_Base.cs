// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 20:34:17
// 版本：V1.1
// 描述：背包基础功能类（存储、使用、添加、移除道具）
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    // 背包内容变化事件（用于刷新UI）
    public event Action OnInventoryChange;

    public int maxInventorySize = 10;               // 背包最大容量
    public List<Inventory_Item> itemList = new List<Inventory_Item>(); // 道具列表

    protected virtual void Awake() { }

    /// <summary>
    /// 尝试使用道具（消耗品）
    /// </summary>
    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);
        if (consumable == null) return;

        // 执行道具效果
        consumable.itemEffect.ExecuteEffect();

        // 堆叠数量处理
        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveItem(consumable);

        OnInventoryChange?.Invoke();
    }

    /// <summary>
    /// 检查背包是否有空位
    /// </summary>
    public bool CanAddItem() => itemList.Count < maxInventorySize;

    /// <summary>
    /// 查找可堆叠的相同道具
    /// </summary>
    public Inventory_Item FindStackable(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }
        return null;
    }

    /// <summary>
    /// 查找可堆叠道具（同FindStackable，兼容旧调用）
    /// </summary>
    public Inventory_Item StackableItem(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
                return stackableItem;
        }
        return null;
    }

    /// <summary>
    /// 添加道具：优先堆叠，无法堆叠则新增
    /// </summary>
    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindStackable(itemToAdd);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        OnInventoryChange?.Invoke();
    }

    /// <summary>
    /// 移除道具
    /// </summary>
    public void RemoveItem(Inventory_Item itemToRemove)
    {
        itemList.Remove(itemToRemove);
        OnInventoryChange?.Invoke();
    }

    /// <summary>
    /// 根据道具数据查找背包内道具
    /// </summary>
    public Inventory_Item FindItem(ItemDataSo itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }

    /// <summary>
    /// 手动触发UI刷新
    /// </summary>
    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}