// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 20:32:11
// 版本：V1.1
// 描述：背包物品实例类（存储道具、堆叠、装备效果、 modifiers）
// ========================================================

using System;

[Serializable]
public class Inventory_Item
{
    private string itemId;

    public ItemDataSo itemData;             // 道具基础数据
    public int stackSize = 1;               // 当前堆叠数量

    public ItemModifier[] modifiers { get; private set; } // 装备属性加成
    public ItemEffect_DataSO itemEffect;     // 道具效果

    /// <summary>
    /// 构造函数：通过道具数据创建背包物品
    /// </summary>
    public Inventory_Item(ItemDataSo itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;
        modifiers = EquipmentData()?.modifiers;

        // 生成唯一ID
        itemId = itemData.itemName + " - " + Guid.NewGuid();
    }

    /// <summary>
    /// 给玩家添加装备属性加成
    /// </summary>
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }

    /// <summary>
    /// 移除玩家身上的装备属性加成
    /// </summary>
    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    /// <summary>
    /// 绑定并启用道具效果
    /// </summary>
    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);

    /// <summary>
    /// 解绑并关闭道具效果
    /// </summary>
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();

    /// <summary>
    /// 获取装备数据（如果是装备类型）
    /// </summary>
    private EquipmentDataSo EquipmentData()
    {
        if (itemData is EquipmentDataSo equipment)
            return equipment;

        return null;
    }

    /// <summary>
    /// 是否可堆叠
    /// </summary>
    public bool CanAddStack() => stackSize < itemData.maxStackSize;

    /// <summary>
    /// 堆叠+1
    /// </summary>
    public void AddStack() => stackSize++;

    /// <summary>
    /// 堆叠-1
    /// </summary>
    public void RemoveStack() => stackSize--;
}