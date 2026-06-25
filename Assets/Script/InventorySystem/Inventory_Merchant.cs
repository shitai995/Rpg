// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:50
// 版本：V1.1
// 描述：商店背包系统（购买、出售、随机刷新商品）
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class Inventory_Merchant : Inventory_Base
{
    private Inventory_Player inventory;

    [SerializeField] private ItemListDataSO shopData;    // 商店可出售的物品列表
    [SerializeField] private int minItemsAmount = 4;     // 最少刷新物品数量

    protected override void Awake()
    {
        base.Awake();
        FillShopList(); // 初始化商店物品
    }

    // 尝试购买物品（支持批量购买）
    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.stackSize : 1;

        for (int i = 0; i < amountToBuy; i++)
        {
            // 金币不足
            if (inventory.gold < itemToBuy.buyPrice)
            {
                Debug.Log("Not enough money!");
                return;
            }

            // 材料 → 存入材料库
            if (itemToBuy.itemData.itemType == ItemType.Material)
            {
                inventory.storage.AddMaterialToStash(itemToBuy);
            }
            // 其他物品 → 加入背包
            else
            {
                if (inventory.CanAddItem(itemToBuy))
                {
                    var itemToAdd = new Inventory_Item(itemToBuy.itemData);
                    inventory.AddItem(itemToAdd);
                }
            }

            inventory.gold -= itemToBuy.buyPrice;

            // 移除商店中的一个物品
            Inventory_Item actualItem = itemList.Find(item => item.itemData == itemToBuy.itemData);
            if (actualItem != null)
                RemoveOneItem(actualItem);
        }

        TriggerUpdateUI();
    }

    // 尝试出售物品（支持批量出售）
    public void TrySellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;

        for (int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);

            inventory.gold += sellPrice;
            inventory.RemoveOneItem(itemToSell);
        }

        TriggerUpdateUI();
    }

    // 随机刷新商店物品列表
    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> possibleItems = new List<Inventory_Item>();

        // 生成所有可购买物品
        foreach (var itemData in shopData.itemList)
        {
            int randomizedStack = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStack = Mathf.Clamp(randomizedStack, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStack;

            possibleItems.Add(itemToAdd);
        }

        // 随机选择一批物品上架
        int randomItemAmount = Random.Range(minItemsAmount, maxInventorySize + 1);
        int finalAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);

        for (int i = 0; i < finalAmount; i++)
        {
            var randomIndex = Random.Range(0, possibleItems.Count);
            var item = possibleItems[randomIndex];

            if (CanAddItem(item))
            {
                possibleItems.Remove(item);
                AddItem(item);
            }
        }

        TriggerUpdateUI();
    }

    // 绑定玩家背包
    public void SetInventory(Inventory_Player inventory) => this.inventory = inventory;
}