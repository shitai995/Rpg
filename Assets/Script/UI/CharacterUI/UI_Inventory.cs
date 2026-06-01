// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 21:49:35
// 版本：V1.1
// 描述：背包UI显示类（同步道具与装备到界面）
// ========================================================

using TMPro;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private UI_EquipSlotParent equipSlotParent;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }
    private void OnEnable()
    {
        if (inventory == null)
            return;

        UpdateUI();
    }
    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        equipSlotParent.UpdateEquipmentSlots(inventory.equipList);
        goldText.text = inventory.gold.ToString("NO") + "g.";
    }

}