// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:22:43
// 版本：V1.1
// 描述：实体掉落物管理器，处理物品掉落、概率与稀有度限制
// ========================================================

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 实体掉落管理组件
/// </summary>
public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab; // 掉落物预制体
    [SerializeField] private ItemListDataSO dropData;  // 掉落物品配置表

    [Header("掉落限制")]
    [SerializeField] private int maxRarityAmount = 1200; // 稀有度总值上限
    [SerializeField] private int maxItemsToDrop = 3;      // 最大掉落物品数量
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            DropItems();
    }
    /// <summary>
    /// 执行物品掉落逻辑
    /// </summary>
    public virtual void DropItems()
    {
        if (dropData == null)
        {
            Debug.Log($"请为 {gameObject.name} 配置掉落数据");
            return;
        }

        List<ItemDataSO> itemsToDrop = RollDrops();
        int dropCount = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for (int i = 0; i < dropCount; i++)
        {
            CreateItemDrop(itemsToDrop[i]);
        }
    }

    /// <summary>
    /// 生成掉落物实例
    /// </summary>
    public void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    /// <summary>
    /// 掉落概率判定与筛选
    /// </summary>
    public List<ItemDataSO> RollDrops()
    {
        List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();
        float remainingRarity = maxRarityAmount;

        // 1. 根据掉落概率筛选候选物品
        foreach (var item in dropData.itemList)
        {
            float dropChance = item.GetDropChance();
            if (Random.Range(0, 100) <= dropChance)
                possibleDrops.Add(item);
        }

        // 2. 按稀有度从高到低排序
        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        // 3. 按稀有度总值上限筛选最终掉落列表
        foreach (var item in possibleDrops)
        {
            if (remainingRarity > item.itemRarity)
            {
                finalDrops.Add(item);
                remainingRarity -= item.itemRarity;
            }
        }

        return finalDrops;
    }
}