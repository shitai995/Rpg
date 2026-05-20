// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-15 20:31:59
// 版本：V1.1
// 描述：玩家属性面板UI管理器
// ========================================================

using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatSlots;  // 所有属性槽子组件
    private Inventory_Player inventory; // 玩家背包（监听装备变化）

    private void Awake()
    {
        // 获取所有子属性槽
        uiStatSlots = GetComponentsInChildren<UI_StatSlot>();
        // 绑定玩家背包
        inventory = FindFirstObjectByType<Inventory_Player>();
        // 监听背包/装备变化，刷新属性面板
        inventory.OnInventoryChange += UpdateStatsUI;
    }

    private void Start()
    {
        // 游戏启动时初始化刷新一次
        UpdateStatsUI();
    }

    /// <summary>
    /// 刷新所有属性槽的数值显示
    /// </summary>
    private void UpdateStatsUI()
    {
        foreach (var statSlot in uiStatSlots)
        {
            statSlot.UpdateStatValue();
        }
    }
}