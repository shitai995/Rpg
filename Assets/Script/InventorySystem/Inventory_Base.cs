// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 20:34:17
// 版本：V1.1
// 描述：背包基础功能类（存储、使用、添加、移除道具）
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有背包的基类，实现通用物品管理逻辑
/// </summary>
public class Inventory_Base : MonoBehaviour,ISaveable
{
    protected Player player;
    // 背包内容变化事件
    public event Action OnInventoryChange;

    [Header("背包设置")]
    public int maxInventorySize = 10;          // 最大格子数
    public List<Inventory_Item> itemList = new List<Inventory_Item>(); // 物品列表

    [Header("ITEM DATA BASE")]
    [SerializeField] protected ItemListDataSO itemDataBase;
    protected virtual void Awake() 
    {
        player = GetComponent<Player>();    
    }

    /// <summary> 尝试使用消耗品 </summary>
    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = itemList.Find(item => item == itemToUse);
        if (consumable == null) return;
        if (consumable.itemEffect.CanBeUsed(player) == false)
            return;

        consumable.itemEffect.ExecuteEffect();

        if (consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }

    /// <summary> 是否能添加该物品（可堆叠 or 有空位） </summary>
    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable = FindStackable(itemToAdd) != null;
        return hasStackable || itemList.Count < maxInventorySize;
    }

    /// <summary> 寻找可堆叠的同类物品 </summary>
    public Inventory_Item FindStackable(Inventory_Item itemToAdd)
    {
        return itemList.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());
    }

    /// <summary> 添加物品（自动堆叠） </summary>
    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item itemInInventory = FindStackable(itemToAdd);

        if (itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        OnInventoryChange?.Invoke();
    }

    /// <summary> 移除一个物品 </summary>
    public void RemoveOneItem(Inventory_Item itemToRemove)
    {
        Inventory_Item itemInInventory = itemList.Find(item => item == itemToRemove);

        if (itemInInventory == null)
        {
            Debug.LogWarning($"尝试删除不存在的物品: {itemToRemove.itemData.name}");
            return;
        }

        if (itemInInventory.stackSize > 1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(itemInInventory);

        OnInventoryChange?.Invoke();
    }

    /// <summary> 移除整个堆叠 </summary>
    public void RemoveFullStack(Inventory_Item itemToRemove)
    {
        for (int i = 0; i < itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }
    /// <summary>
    /// 扣除指定数量道具
    /// </summary>
    /// <param name="itemToRemove">目标道具配置</param>
    /// <param name="amount">要扣除总数</param>
    public void RemoveItemAmount(ItemDataSO itemToRemove, int amount)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Inventory_Item item = itemList[i];
            // 道具不匹配跳过
            if (item.itemData != itemToRemove)
                continue;

            // 当前格子最多可扣除数量
            int removeCount = Mathf.Min(amount, item.stackSize);
            // 逐个移除单份道具
            for (int j = 0; j < removeCount; j++)
            {
                RemoveOneItem(item);
                amount--;
                if (amount <= 0) break;
            }
            // 扣完目标数量直接退出循环
            if (amount <= 0) break;
        }
    }

    /// <summary>
    /// 判断背包是否拥有足够数量指定道具
    /// </summary>
    /// <param name="itemToCheck">道具配置</param>
    /// <param name="amount">需要的数量</param>
    /// <returns>true=道具充足</returns>
    public bool HasItemAmount(ItemDataSO itemToCheck, int amount)
    {
        int total = 0;
        foreach (var item in itemList)
        {
            if (item.itemData == itemToCheck)
                total += item.stackSize;
            // 累计满足需求提前返回
            if (total >= amount)
                return true;
        }
        return false;
    }
    /// <summary> 在背包中查找物品 </summary>
    public Inventory_Item FindItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }
    public Inventory_Item FindSameItem(Inventory_Item itemToFind)
    {
        return itemList.Find(item => item.itemData == itemToFind.itemData);
    }
    /// <summary> 手动触发UI刷新 </summary>
    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();

    public virtual void LoadData(GameData data)
    {
        
    }

    public virtual void SaveData(ref GameData data)
    {
    }
}