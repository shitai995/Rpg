// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-14 14:31:25
// 版本：V1.1
// 描述：装备槽UI组件（点击卸下装备）
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot
{
    [Tooltip("装备槽类型（武器/护甲/饰品等）")]
    public ItemType slotType;

    // 编辑器自动重命名插槽
    private void OnValidate()
    {
        gameObject.name = "UI_EquipmentSlot - " + slotType.ToString();
    }

    /// <summary>
    /// 点击装备槽 → 卸下当前装备
    /// </summary>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;

        inventory.UnequipItem(itemInSlot);
    }
}