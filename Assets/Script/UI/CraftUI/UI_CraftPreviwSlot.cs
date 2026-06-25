// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：合成预览材料槽UI（显示材料名、图标、数量）
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviwSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialNameValue;

    // 设置材料预览：图标、名称、当前数量/所需数量
    public void SetupPreviwSlot(ItemDataSO itemData, int availableAmount, int requiredAmount)
    {
        materialIcon.sprite = itemData.itemIcon;
        materialNameValue.text = itemData.itemName + " - " + availableAmount + "/" + requiredAmount;
    }
}