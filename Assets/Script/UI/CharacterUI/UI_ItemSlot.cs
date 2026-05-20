// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 21:41:03
// 版本：V1.1
// 描述：UI物品槽基础类（背包/装备通用，处理显示、点击、提示）
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }  // 当前槽位内的物品
    protected Inventory_Player inventory;                   // 玩家背包引用
    protected UI ui;                                        // 总UI管理器
    protected RectTransform rect;                           // 自身矩形变换（用于提示框定位）

    [Header("物品槽UI设置")]
    [SerializeField] private Image itemIcon;               // 物品图标
    [SerializeField] private TextMeshProUGUI itemStackSize;// 堆叠数量文本

    protected void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    /// <summary>
    /// 点击物品槽：根据物品类型执行使用/装备逻辑
    /// </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // 无物品 / 材料类型  不处理
        if (itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)
            return;

        // 消耗品  使用
        if (itemInSlot.itemData.itemType == ItemType.Consumable)
        {
            if (itemInSlot.itemEffect.CanBeUsed() == false)
                return;

            inventory.TryUseItem(itemInSlot);
        }
        // 装备  穿戴
        else
        {
            inventory.TryEquipItem(itemInSlot);
        }

        // 使用后物品为空  关闭提示框
        if (itemInSlot == null)
            ui.itemToolTip.ShowToolTip(false, null);
    }

    /// <summary>
    /// 更新槽位UI显示（图标、堆叠数、清空状态）
    /// </summary>
    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        // 无物品  清空显示
        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        // 有物品  显示图标与堆叠
        Color color = Color.white;
        color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    /// <summary>
    /// 鼠标进入  显示物品提示框
    /// </summary>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    /// <summary>
    /// 鼠标离开  关闭物品提示框
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}