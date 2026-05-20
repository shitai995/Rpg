// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemToCraft;
    [SerializeField] private UI_CraftPreviw craftPreviw;


    [SerializeField] private Image craftItemIcon;
    [SerializeField] private TextMeshProUGUI craftItemName;


    public void SetupButton(ItemDataSO craftData)
    {
        this.itemToCraft = craftData;
        craftItemIcon.sprite = craftData.itemIcon;
        craftItemName.text = craftData.itemName;
    }

    public void UpdateCraftPreviw() => craftPreviw.UpdateCraftPreviw(itemToCraft);

}
