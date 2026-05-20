// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:23:24
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class Player_DropManager : Entity_DropManager
{
    [Header("PLAYER Drop Details")]
    [Range(0,100)]
    [SerializeField] private float chanceToLooseItem = 90f;
    private Inventory_Player inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory_Player>();
    }

    public override void DropItems()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.itemList);
        List<Inventory_EquipmentSlot> eqiupCopy = new List<Inventory_EquipmentSlot>(inventory.equipList);

        foreach (var item in inventoryCopy)
        {
            if (Random.Range(0, 100) < chanceToLooseItem)
            {
                CreateItemDrop(item.itemData);
                inventory.RemoveFullStack(item);
            }
        }

        foreach (var equip in eqiupCopy)
        {
            if (Random.Range(0, 100) < chanceToLooseItem && equip.HasItem())
            {
                var item = equip.GetEquipedItem();

                CreateItemDrop(item.itemData);
                inventory.UnequipItem(item);
                inventory.RemoveFullStack(item);
            }
        }
    }
}
