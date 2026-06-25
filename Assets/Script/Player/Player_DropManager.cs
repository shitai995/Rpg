// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:23:24
// 版本：V1.1
// 描述：玩家死亡掉落管理器（随机丢失背包/装备物品）
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class Player_DropManager : Entity_DropManager
{
    [Header("玩家掉落设置")]
    [Range(0, 100)]
    [SerializeField] private float chanceToLooseItem = 90f; // 物品丢失概率
    private Inventory_Player inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory_Player>();
    }

    // 玩家死亡时调用：掉落物品与装备
    public override void DropItems()
    {
        // 复制列表避免遍历过程中修改原数据
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.itemList);
        List<Inventory_EquipmentSlot> equipCopy = new List<Inventory_EquipmentSlot>(inventory.equipList);

        // 掉落背包物品
        foreach (var item in inventoryCopy)
        {
            if (Random.Range(0, 100) < chanceToLooseItem)
            {
                CreateItemDrop(item.itemData);
                inventory.RemoveFullStack(item);
            }
        }

        // 掉落装备物品
        foreach (var equip in equipCopy)
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