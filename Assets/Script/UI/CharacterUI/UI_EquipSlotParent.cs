// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:18:25
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipSlot[] equipSlots;

    public void UpdateEquipmentSlots(List<Inventory_EquipmentSlot> equipList)
    {
        if(equipSlots == null)
            equipSlots = GetComponentsInChildren<UI_EquipSlot>();

        for (int i = 0; i < equipSlots.Length; i++)
        {
            var playerEquipSlot = equipList[i];

            if (playerEquipSlot.HasItem() == false)
                equipSlots[i].UpdateSlot(null);
            else
                equipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }
}
