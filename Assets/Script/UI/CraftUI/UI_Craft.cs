// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：合成界面UI管理器
// ========================================================

using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player inventory;

    private UI_CraftPreviw craftPreviwUI;
    private UI_CraftSlot[] craftSlots;
    private UI_CraftListButton[] craftListButtons;

    // 初始化合成界面，绑定储物栏数据
    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.playerInventory;
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();

        craftPreviwUI = GetComponentInChildren<UI_CraftPreviw>(true);
        craftPreviwUI.SetupCraftPreviw(storage);
        SetupCraftListButtons();
    }

    // 初始化合成列表按钮，隐藏所有合成槽
    private void SetupCraftListButtons()
    {
        craftSlots = GetComponentsInChildren<UI_CraftSlot>(true);
        craftListButtons = GetComponentsInChildren<UI_CraftListButton>(true);

        foreach (var slot in craftSlots)
            slot.gameObject.SetActive(false);

        foreach (var button in craftListButtons)
            button.SetCraftSlots(craftSlots);
    }

    // 更新背包物品显示
    private void UpdateUI() => inventoryParent.UpdateSlots(inventory.itemList);
}