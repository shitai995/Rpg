// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:50
// 版本：V1.1
// 描述：储物栏系统（存储、材料库、合成、玩家<->储物栏转移）
// ========================================================

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory { get; private set; }
    public List<Inventory_Item> materialStash; // 材料专用库

    // 合成物品：消耗材料 + 加入玩家背包
    public void CraftItem(Inventory_Item itemToCraft)
    {
        ConsumeMaterials(itemToCraft);
        playerInventory.AddItem(itemToCraft);
    }

    // 是否满足合成条件
    public bool CanCraftItem(Inventory_Item itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && playerInventory.CanAddItem(itemToCraft);
    }

    // 消耗合成材料（从背包、储物栏、材料库依次扣除）
    private void ConsumeMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRecipe)
        {
            int amountNeeded = requiredItem.stackSize;

            amountNeeded -= ConsumedMaterialsAmount(playerInventory.itemList, requiredItem);
            if (amountNeeded > 0)
                amountNeeded -= ConsumedMaterialsAmount(itemList, requiredItem);
            if (amountNeeded > 0)
                amountNeeded -= ConsumedMaterialsAmount(materialStash, requiredItem);
        }
    }

    // 从指定列表消耗材料
    private int ConsumedMaterialsAmount(List<Inventory_Item> itemList, Inventory_Item neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;

        for (int i = itemList.Count - 1; i >= 0; i--)
        {
            if (itemList[i].itemData != neededItem.itemData)
                continue;

            int removeAmount = Mathf.Min(itemList[i].stackSize, amountNeeded - consumedAmount);
            itemList[i].stackSize -= removeAmount;
            consumedAmount += removeAmount;

            if (itemList[i].stackSize <= 0)
                itemList.RemoveAt(i);

            if (consumedAmount >= amountNeeded)
                break;
        }

        return consumedAmount;
    }

    // 是否拥有足够材料
    private bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredMaterial in itemToCraft.itemData.craftRecipe)
        {
            if (GetAvailableAmountOf(requiredMaterial.itemData) < requiredMaterial.stackSize)
                return false;
        }
        return true;
    }

    // 获取所有来源（背包/储物/材料库）的物品总数量
    public int GetAvailableAmountOf(ItemDataSO requiredItem)
    {
        int amount = 0;

        foreach (var item in playerInventory.itemList)
            if (item.itemData == requiredItem) amount += item.stackSize;

        foreach (var item in itemList)
            if (item.itemData == requiredItem) amount += item.stackSize;

        foreach (var item in materialStash)
            if (item.itemData == requiredItem) amount += item.stackSize;

        return amount;
    }

    // 添加物品到材料库（自动堆叠）
    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if (stackableItem != null)
            stackableItem.AddStack();
        else
        {
            var newItemToAdd = new Inventory_Item(itemToAdd.itemData);
            materialStash.Add(newItemToAdd);

        }

        TriggerUpdateUI();
        materialStash = materialStash.OrderBy(item => item.itemData.name).ToList();
    }

    // 查找材料库可堆叠物品
    public Inventory_Item StackableInStash(Inventory_Item itemToAdd)
    {
        return materialStash.Find(item => item.itemData == itemToAdd.itemData && item.CanAddStack());

    }

    // 绑定玩家背包
    public void SetInventory(Inventory_Player inventory) => this.playerInventory = inventory;

    // 玩家背包 → 储物栏
    public void FromPlayerToStorage(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);
                playerInventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }
        TriggerUpdateUI();
    }

    // 储物栏 → 玩家背包
    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.stackSize : 1;

        for (int i = 0; i < transferAmount; i++)
        {
            if (playerInventory.CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);
                RemoveOneItem(item);
                playerInventory.AddItem(itemToAdd);
            }
        }
        TriggerUpdateUI();
    }
    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);

        data.storageItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveId;


                if (data.storageItems.ContainsKey(saveId) == false)
                    data.storageItems[saveId] = 0;

                data.storageItems[saveId] += item.stackSize;
            }
        }

        data.storageMaterials.Clear();

        foreach (var item in materialStash)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveId;


                if (data.storageMaterials.ContainsKey(saveId) == false)
                    data.storageMaterials[saveId] = 0;

                data.storageMaterials[saveId] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        itemList.Clear();
        materialStash.Clear();

        foreach (var entry in data.storageItems)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddItem(itemToLoad);
            }
        }

        foreach (var entry in data.storageMaterials)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                Inventory_Item itemToLoad = new Inventory_Item(itemData);
                AddMaterialToStash(itemToLoad);
            }

        }
    }
}