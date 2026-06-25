// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 21:49:35
// 版本：V1.1
// 描述：背包界面UI管理，同步道具、装备与金币数据至界面
// ========================================================

using TMPro;
using UnityEngine;

/// <summary>
/// 背包UI控制器
/// </summary>
public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent; // 背包格子父物体
    [SerializeField] private UI_EquipSlotParent equipSlotParent;    // 装备槽父物体
    [SerializeField] private TextMeshProUGUI goldText;              // 金币文本

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory_Player>();
        // 订阅背包变更事件
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    private void OnEnable()
    {
        if (inventory == null) return;
        UpdateUI();
    }

    /// <summary>
    /// 刷新背包、装备、金币整体界面
    /// </summary>
    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        equipSlotParent.UpdateEquipmentSlots(inventory.equipList);
        goldText.text = $"{inventory.gold:N0} g.";
    }
}