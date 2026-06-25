// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:18:25
// 版本：V1.1
// 描述：装备槽父容器（批量更新所有装备栏显示）
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipSlot[] equipSlots;

    // 批量更新所有装备槽UI
    public void UpdateEquipmentSlots(List<Inventory_EquipmentSlot> equipList)
    {
        if (equipSlots == null)
            equipSlots = GetComponentsInChildren<UI_EquipSlot>();

        for (int i = 0; i < equipSlots.Length; i++)
        {
            var playerEquipSlot = equipList[i];

            if (!playerEquipSlot.HasItem())
                equipSlots[i].UpdateSlot(null);
            else
                equipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }
}