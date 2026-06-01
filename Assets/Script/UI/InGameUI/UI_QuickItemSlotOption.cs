// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:31:09
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;
    
    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot, Inventory_Item itemToSet)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateSlot(itemToSet);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);
        ui.inGameUI.HideQuickItemOptions();
    }
}
