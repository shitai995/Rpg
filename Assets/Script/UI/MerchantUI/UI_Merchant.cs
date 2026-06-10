// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:05
// 版本：V1.1
// 描述：商店UI管理器（同步商店/玩家背包/装备栏显示）
// ========================================================

using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private TextMeshProUGUI goldText;

    [Space]
    [SerializeField] private UI_ItemSlotParent merchantSlots;   // 商店物品槽父物体
    [SerializeField] private UI_ItemSlotParent inventorySlots; // 玩家背包槽父物体
    [SerializeField] private UI_EquipSlotParent equipSlots;     // 玩家装备槽父物体

    // 初始化商店UI，绑定数据
    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        // 先取消旧订阅，防止重复注册
        if (this.inventory != null) this.inventory.OnInventoryChange -= UpdateSlotUI;
        if (this.merchant != null) this.merchant.OnInventoryChange -= UpdateSlotUI;

        this.merchant = merchant;
        this.inventory = inventory;

        this.inventory.OnInventoryChange += UpdateSlotUI;
        this.merchant.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        // 给所有商店槽位绑定商店逻辑
        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();
        foreach (var slot in merchantSlots)
            slot.SetupMerchantUI(merchant);
    }

    // 更新所有UI显示
    private void UpdateSlotUI()
    {
        if (inventory == null)
            return;

        merchantSlots.UpdateSlots(merchant.itemList);
        inventorySlots.UpdateSlots(inventory.itemList);
        equipSlots.UpdateEquipmentSlots(inventory.equipList);
        goldText.text = inventory.gold.ToString("N0") + "g.";

    }
}