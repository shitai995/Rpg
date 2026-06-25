// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:05
// 版本：V1.1
// 描述：商店物品槽（支持购买/出售/右键快捷交易）
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;

    // 槽位类型：商店出售/玩家背包
    public enum MerchantSlotType { MerchantSlot, PlayerSlot }
    public MerchantSlotType slotType;

    // 点击交互：购买/出售/使用装备
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        // 玩家背包槽 → 右键出售
        if (slotType == MerchantSlotType.PlayerSlot)
        {
            if (rightButton)
            {
                bool sellFullStack = Input.GetKey(KeyCode.LeftControl);
                merchant.TrySellItem(itemInSlot, sellFullStack);
            }
            else if (leftButton)
            {
                base.OnPointerDown(eventData);
            }
        }
        // 商店槽右键购买
        else if (slotType == MerchantSlotType.MerchantSlot)
        {
            if (leftButton)
                return;

            bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(itemInSlot, buyFullStack);
        }

        ui.itemToolTip.ShowToolTip(false, null);
    }

    // 鼠标悬浮显示道具提示
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        if (slotType == MerchantSlotType.MerchantSlot)
            ui.itemToolTip.ShowToolTip(true, rect, itemInSlot, true, true);
        else
            ui.itemToolTip.ShowToolTip(true, rect, itemInSlot, false, true);
    }

    // 绑定商店管理器
    public void SetupMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;
}