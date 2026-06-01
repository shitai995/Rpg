// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-15 20:31:59
// 版本：V1.1
// 描述：玩家属性面板UI管理器
// ========================================================

using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatSlots;
    private Inventory_Player inventory;

    private void Awake()
    {
        uiStatSlots = GetComponentsInChildren<UI_StatSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateStatsUI;
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach (var statSlot in uiStatSlots)
            statSlot.UpdateStatValue();
    }
}