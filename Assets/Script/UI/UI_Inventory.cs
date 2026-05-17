// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 21:49:35
// 版本：V1.1
// 描述：背包UI显示类（同步道具与装备到界面）
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;          // 玩家背包数据

    private UI_ItemSlot[] uiItemSlots;           // 背包物品格子数组
    private UI_EquipSlot[] uiEquipSlots;         // 装备槽格子数组

    [SerializeField] private Transform uiItemSlotParent;   // 背包格子父物体
    [SerializeField] private Transform uiEquipSlotParent;  // 装备槽父物体

    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();

        // 绑定玩家背包并监听变化事件
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        // 初始化刷新UI
        UpdateUI();
    }

    /// <summary>
    /// 统一刷新背包与装备UI
    /// </summary>
    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    /// <summary>
    /// 刷新装备槽UI：同步数据到装备界面
    /// </summary>
    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;

        for (int i = 0; i < uiEquipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if (!playerEquipSlot.HasItem())
                uiEquipSlots[i].UpdateSlot(null);
            else
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }

    /// <summary>
    /// 刷新背包格子UI：同步数据到背包界面
    /// </summary>
    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for (int i = 0; i < uiItemSlots.Length; i++)
        {
            if (i < itemList.Count)
                uiItemSlots[i].UpdateSlot(itemList[i]);
            else
                uiItemSlots[i].UpdateSlot(null);
        }
    }
}