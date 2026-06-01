// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:31:09
// 版本：V1.1
// 描述：快捷道具选择选项UI，点击后为对应快捷栏格子绑定道具并关闭弹窗
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 快捷道具选择项
/// </summary>
public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;

    /// <summary>
    /// 初始化选项数据与关联的快捷格子
    /// </summary>
    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot, Inventory_Item itemToSet)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateSlot(itemToSet);
    }

    /// <summary>
    /// 点击选中道具，绑定到快捷栏并隐藏选择面板
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);
        ui.inGameUI.HideQuickItemOptions();
    }
}