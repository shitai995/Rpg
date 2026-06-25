// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:20:39
// 版本：V1.1
// 描述：储物栏专用物品槽（支持玩家背包 ↔ 储物栏双向转移）
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    // 槽位类型：储物栏槽 / 玩家背包槽
    public enum StorageSlotType { StorageSlot, PlayerInventorySlot }
    public StorageSlotType slotType;

    // 绑定储物系统
    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    // 点击转移物品
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        // Ctrl + 左键 = 转移整组物品
        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);

        // 储物栏 → 玩家
        if (slotType == StorageSlotType.StorageSlot)
            storage.FromStorageToPlayer(itemInSlot, transferFullStack);

        // 玩家 → 储物栏
        if (slotType == StorageSlotType.PlayerInventorySlot)
            storage.FromPlayerToStorage(itemInSlot, transferFullStack);

        // 关闭提示
        ui.itemToolTip.ShowToolTip(false, null);
    }
}