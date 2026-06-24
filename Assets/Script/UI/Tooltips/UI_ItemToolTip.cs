// ========================================================
// 作者：娇娇
// 版本：V1.1
// 描述：物品提示框UI，显示道具名称、类型、属性、效果描述
// ========================================================

using TMPro;
using UnityEngine;

/// <summary>
/// 物品悬浮提示：显示名称（按稀有度变色）、类型、属性、价格、描述
/// </summary>
public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;    // 物品名称
    [SerializeField] private TextMeshProUGUI itemType;    // 物品类型
    [SerializeField] private TextMeshProUGUI itemInfo;    // 物品详情（属性/效果）

    [SerializeField] private TextMeshProUGUI itemPrice;   // 价格
    [SerializeField] private Transform merchnatInfo;      // 商人界面专用布局
    [SerializeField] private Transform inventoryInfo;     // 背包界面专用布局

    /// <summary>
    /// 显示物品提示
    /// </summary>
    /// <param name="show">是否显示</param>
    /// <param name="targetRect">跟随的槽位</param>
    /// <param name="itemToShow">要显示的物品</param>
    /// <param name="buyPrice">true=显示购买价 false=显示出售价</param>
    /// <param name="showMerchantInfo">是否显示商人界面的价格布局</param>
    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow, bool buyPrice = false, bool showMerchantInfo = false, bool showControls = false)
    {
        base.ShowToolTip(show, targetRect);

        if (showControls)
        {
            // 切换商人/普通界面布局
            merchnatInfo.gameObject.SetActive(showMerchantInfo);
            inventoryInfo.gameObject.SetActive(!showMerchantInfo);
        }
        else
        {
            merchnatInfo.gameObject.SetActive(false);
            inventoryInfo.gameObject.SetActive(false);
        }


        // 计算价格
        int price = buyPrice ? itemToShow.buyPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string fullStackPrice = $"Price:{price}x{itemToShow.stackSize} - {totalPrice}g.";
        string singleStackPrice = $"Price:{price}g.";

        // 赋值UI
        itemPrice.text = itemToShow.stackSize > 1 ? fullStackPrice : singleStackPrice;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = itemToShow.GetItemInfo();

        // 名称按稀有度染色
        string color = GetColorByRarity(itemToShow.itemData.itemRarity);
        itemName.text = GetColoredText(color, itemToShow.itemData.itemName);
    }

    /// <summary>
    /// 根据物品稀有度返回颜色名称
    /// </summary>
    private string GetColorByRarity(int rarity)
    {
        if (rarity <= 100) return "white";   // 普通
        if (rarity <= 300) return "green";   //  uncommon
        if (rarity <= 600) return "blue";    // 稀有
        if (rarity <= 850) return "purple";  // 史诗
        return "orange";                     // 传说
    }

    /// <summary>
    /// 给文本添加颜色标签
    /// </summary>
    private string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }
}