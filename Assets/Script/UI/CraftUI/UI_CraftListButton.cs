// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：合成分类按钮（切换显示不同合成配方列表）
// ========================================================

using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craftData; // 该分类对应的合成物品列表
    private UI_CraftSlot[] craftSlots;                  // 合成槽位数组

    // 传入所有合成槽
    public void SetCraftSlots(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    // 点击按钮 → 更新显示该分类下的可合成物品
    public void UpdateCraftSlots()
    {
        if (craftData == null)
        {
            Debug.Log("You need to assign craft list data!");
            return;
        }

        // 先隐藏所有槽位
        foreach (var slot in craftSlots)
            slot.gameObject.SetActive(false);

        // 显示当前分类的物品
        for (int i = 0; i < craftData.itemList.Length; i++)
        {
            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(craftData.itemList[i]);
        }
    }
}