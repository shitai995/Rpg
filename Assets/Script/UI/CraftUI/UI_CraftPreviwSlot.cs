// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviwSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialNameValue;

    public void SetupPreviwSlot(ItemDataSO itemData, int avaliableAmount, int requiredAmount)
    {
        materialIcon.sprite = itemData.itemIcon;
        materialNameValue.text = itemData.itemName + " - " + avaliableAmount + "/" + requiredAmount;
    }
}
