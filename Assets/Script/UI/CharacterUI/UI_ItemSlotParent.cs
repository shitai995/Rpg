// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:18:25
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlotParent : MonoBehaviour
{
    private UI_ItemSlot[] slots;


    public void UpdateSlots(List<Inventory_Item> itemList)
    {
        if(slots == null)
            slots = GetComponentsInChildren<UI_ItemSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemList.Count)
            {
                slots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                slots[i].UpdateSlot(null);
            }
        }
    }
}
