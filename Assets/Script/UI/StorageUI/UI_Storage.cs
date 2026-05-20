// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:20:39
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;
    [SerializeField] private UI_ItemSlotParent materialStashParent;

    public void SetupStorageUI(Inventory_Storage storage)
    {
        this.storage = storage;
        inventory = storage.playerInventory;

        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();

        foreach (var slot in storageSlots)
            slot.SetStorage(storage);
    }

    private void OnEnable()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (storage == null)
            return;

        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
        materialStashParent.UpdateSlots(storage.materialStash);
    }

}
