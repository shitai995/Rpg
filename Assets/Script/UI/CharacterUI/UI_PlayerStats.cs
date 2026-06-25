// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-15 20:31:59
// 版本：V1.1
// 描述：玩家属性面板UI管理器，监听背包变化并刷新所有属性显示
// ========================================================

using UnityEngine;

/// <summary>
/// 玩家属性面板UI控制器
/// </summary>
public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatSlots;
    private Inventory_Player inventory;

    private void Awake()
    {
        // 获取所有子物体属性槽
        uiStatSlots = GetComponentsInChildren<UI_StatSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();

        // 订阅背包变更事件，同步刷新属性
        inventory.OnInventoryChange += UpdateStatsUI;
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    /// <summary>
    /// 刷新全部属性数值
    /// </summary>
    private void UpdateStatsUI()
    {
        foreach (var slot in uiStatSlots)
        {
            slot.UpdateStatValue();
        }
    }
}