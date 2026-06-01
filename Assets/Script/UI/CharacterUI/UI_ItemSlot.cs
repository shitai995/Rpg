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

/// <summary>
/// 物品槽基础组件，负责显示物品、处理点击、悬浮提示
/// </summary>
public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;

    [Header("UI Slot Setup")]
    [SerializeField] protected GameObject defaultIcon;
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemStackSize;

    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    /// <summary> 点击物品槽：使用/装备/删除物品 </summary>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)
            return;

        bool alternativeInput = Input.GetKey(KeyCode.LeftControl);

        // Ctrl + 左键 → 删除物品
        if (alternativeInput)
        {
            inventory.RemoveOneItem(itemInSlot);
        }
        else
        {
            // 消耗品 → 使用
            if (itemInSlot.itemData.itemType == ItemType.Consumable)
            {


                inventory.TryUseItem(itemInSlot);
            }
            // 装备 → 穿戴
            else
                inventory.TryEquipItem(itemInSlot);
        }

        if (itemInSlot == null)
            ui.itemToolTip.ShowToolTip(false, null);
    }

    /// <summary> 更新槽位显示内容 </summary>
    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (defaultIcon != null)
            defaultIcon.gameObject.SetActive(itemInSlot == null);

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
      
            return;
        }
       
        Color color = Color.white; color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    /// <summary> 鼠标进入 → 显示提示 </summary>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    /// <summary> 鼠标离开 → 关闭提示 </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
}