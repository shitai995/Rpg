// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:14
// 版本：V1.1
// 描述：合成预览界面UI（显示配方、材料、合成按钮）
// ========================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreviw : MonoBehaviour
{
    private Inventory_Item itemToCraft;          // 待合成物品
    private Inventory_Storage storage;          // 储物库
    private UI_CraftPreviwSlot[] craftPreviwSlots; // 材料槽数组

    [Header("物品预览")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;

    // 初始化合成预览界面
    public void SetupCraftPreviw(Inventory_Storage storage)
    {
        this.storage = storage;

        craftPreviwSlots = GetComponentsInChildren<UI_CraftPreviwSlot>();
        foreach (var slot in craftPreviwSlots)
            slot.gameObject.SetActive(false);
    }

    // 点击确认合成
    public void ConfirmCraft()
    {
        if (itemToCraft == null)
        {
            buttonText.text = "Pick an item.";
            return;
        }

        if (storage.CanCraftItem(itemToCraft))
            storage.CraftItem(itemToCraft);

        UpdateCraftPreviwSlots();
    }

    // 更新预览内容（物品信息）
    public void UpdateCraftPreviw(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemData.itemIcon;
        itemName.text = itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();
        UpdateCraftPreviwSlots();
    }

    // 更新材料显示（已拥有/所需数量）
    private void UpdateCraftPreviwSlots()
    {
        foreach (var slot in craftPreviwSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
            int availableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.stackSize;

            craftPreviwSlots[i].gameObject.SetActive(true);
            craftPreviwSlots[i].SetupPreviwSlot(requiredItem.itemData, availableAmount, requiredAmount);
        }
    }
}