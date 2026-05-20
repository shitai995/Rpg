// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:05
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_EquipSlotParent equipSlots;

    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        this.inventory.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();

        foreach (var slot in merchantSlots)
            slot.SetupMerchantUI(merchant);

    }


    private void UpdateSlotUI()
    {
        if (inventory == null)
            return;

        merchantSlots.UpdateSlots(merchant.itemList);
        inventorySlots.UpdateSlots(inventory.itemList);
        equipSlots.UpdateEquipmentSlots(inventory.equipList);
    }
}
