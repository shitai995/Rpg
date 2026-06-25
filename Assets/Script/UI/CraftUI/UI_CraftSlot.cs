// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：合成槽位UI（显示可合成物品，点击预览配方）
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemToCraft;    // 要合成的物品数据

    [SerializeField] private UI_CraftPreviw craftPreviw;  // 合成预览窗口

    [SerializeField] private Image craftItemIcon;          // 物品图标
    [SerializeField] private TextMeshProUGUI craftItemName;// 物品名称


    // 设置合成按钮显示内容
    public void SetupButton(ItemDataSO craftData)
    {
        this.itemToCraft = craftData;
        craftItemIcon.sprite = craftData.itemIcon;
        craftItemName.text = craftData.itemName;
    }

    // 更新合成预览面板
    public void UpdateCraftPreviw() => craftPreviw.UpdateCraftPreviw(itemToCraft);
}