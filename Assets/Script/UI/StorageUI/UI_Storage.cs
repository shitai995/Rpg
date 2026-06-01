// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:20:39
// 版本：V1.1
// 描述：储物栏UI（同步显示玩家背包、公共储物、材料库）
// ========================================================

using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;    // 玩家背包
    private Inventory_Storage storage;      // 储物系统

    [SerializeField] private UI_ItemSlotParent inventoryParent;    // 玩家背包槽父物体
    [SerializeField] private UI_ItemSlotParent storageParent;       // 储物栏槽父物体
    [SerializeField] private UI_ItemSlotParent materialStashParent; // 材料库槽父物体

    // 初始化储物界面，绑定数据
    public void SetupStorageUI(Inventory_Storage storage)
    {
        this.storage = storage;
        inventory = storage.playerInventory;

        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        // 给所有储物槽绑定储物系统
        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();
        foreach (var slot in storageSlots)
            slot.SetStorage(storage);
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    // 更新所有槽位显示
    private void UpdateUI()
    {
        if (storage == null)
            return;

        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
        materialStashParent.UpdateSlots(storage.materialStash);
    }
}