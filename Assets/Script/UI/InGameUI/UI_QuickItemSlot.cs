// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:31:09
// 版本：V1.1
// 描述：快捷道具格子UI，继承基础物品格子，支持选择道具、点击反馈与弹窗呼出
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 快捷道具栏格子
/// </summary>
public class UI_QuickItemSlot : UI_ItemSlot
{
    private Button button;
    [SerializeField] private Sprite defaultSprite; // 格子默认贴图
    [SerializeField] private int slotNumber;       // 格子序号

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    /// <summary>
    /// 为当前快捷栏格子设置道具
    /// </summary>
    public void SetupQuickSlotItem(Inventory_Item itemToPass)
    {
        inventory.SetQuickItemInSlot(slotNumber, itemToPass);
    }

    /// <summary>
    /// 模拟按钮点击交互反馈
    /// </summary>
    public void SimulateButtonFeedback()
    {
        EventSystem currentEvent = EventSystem.current;
        currentEvent.SetSelectedGameObject(button.gameObject);
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(currentEvent), ExecuteEvents.submitHandler);
    }

    /// <summary>
    /// 刷新快捷格子显示内容
    /// </summary>
    public void UpdateQuickSlotUI(Inventory_Item currentItemInSlot)
    {
        if (currentItemInSlot == null || currentItemInSlot.itemData == null)
        {
            itemIcon.sprite = defaultSprite;
            itemStackSize.text = string.Empty;
            return;
        }

        itemIcon.sprite = currentItemInSlot.itemData.itemIcon;
        itemStackSize.text = currentItemInSlot.stackSize.ToString();
    }

    /// <summary>
    /// 鼠标按下，打开道具选择弹窗
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.inGameUI.OpenQuickItemOptions(this, rect);
    }
}