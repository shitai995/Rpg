// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:31:08
// 版本：V1.1
// 描述：游戏主界面UI管理器，统筹血条、快捷道具栏、技能栏及相关弹窗
// ========================================================

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏内主界面UI
/// </summary>
public class UI_InGame : MonoBehaviour
{
    private Player player;
    private Inventory_Player inventory;
    private UI_SkillSlot[] skillSlots;

    [SerializeField] private RectTransform healthRect;        // 血条背景矩形
    [SerializeField] private Slider healthSlider;             // 血量滑动条
    [SerializeField] private TextMeshProUGUI healthText;       // 血量文字

    [Header("快捷道具栏")]
    [SerializeField] private float yOffsetQuickItemParent = 150; // 道具选择面板Y轴偏移
    [SerializeField] private Transform quickItemOptionsParent;   // 快捷道具选择面板父物体
    private UI_QuickItemSlotOption[] quickItemOptions;           // 可选道具选项
    private UI_QuickItemSlot[] quickItemSlots;                   // 快捷道具格子

    private void Start()
    {
        // 获取所有快捷道具格子
        quickItemSlots = GetComponentsInChildren<UI_QuickItemSlot>();

        // 绑定玩家与血量更新事件
        player = FindFirstObjectByType<Player>();
        player.health.OnHealthUpdate += UpdateHealthBar;

        // 绑定背包相关事件
        inventory = player.inventory;
        inventory.OnInventoryChange += UpdateQuickSlotsUI;
        inventory.OnQuickSlotUsed += PlayQuickSlotFeedback;
    }

    /// <summary>
    /// 播放快捷道具点击反馈
    /// </summary>
    public void PlayQuickSlotFeedback(int slotNumber)
        => quickItemSlots[slotNumber].SimulateButtonFeedback();

    /// <summary>
    /// 刷新快捷道具栏显示
    /// </summary>
    public void UpdateQuickSlotsUI()
    {
        Inventory_Item[] quickItems = inventory.quickItems;
        for (int i = 0; i < quickItems.Length; i++)
            quickItemSlots[i].UpdateQuickSlotUI(quickItems[i]);
    }

    /// <summary>
    /// 打开快捷道具选择弹窗
    /// </summary>
    public void OpenQuickItemOptions(UI_QuickItemSlot quickItemSlot, RectTransform targetRect)
    {
        if (quickItemOptions == null)
            quickItemOptions = quickItemOptionsParent.GetComponentsInChildren<UI_QuickItemSlotOption>(true);

        // 筛选背包内所有消耗品
        List<Inventory_Item> consumables = inventory.itemList.FindAll(item => item.itemData.itemType == ItemType.Consumable);

        // 刷新选项列表
        for (int i = 0; i < quickItemOptions.Length; i++)
        {
            if (i < consumables.Count)
            {
                quickItemOptions[i].gameObject.SetActive(true);
                quickItemOptions[i].SetupOption(quickItemSlot, consumables[i]);
            }
            else
                quickItemOptions[i].gameObject.SetActive(false);
        }

        // 设置弹窗位置
        quickItemOptionsParent.position = targetRect.position + Vector3.up * yOffsetQuickItemParent;
    }

    /// <summary>
    /// 隐藏快捷道具选择弹窗
    /// </summary>
    public void HideQuickItemOptions()
        => quickItemOptionsParent.position = new Vector3(0, 9999);

    /// <summary>
    /// 根据技能类型获取对应技能格子
    /// </summary>
    public UI_SkillSlot GetSkillSlot(SkillType skillType)
    {
        if (skillSlots == null)
            skillSlots = GetComponentsInChildren<UI_SkillSlot>(true);

        foreach (var slot in skillSlots)
        {
            if (slot.skillType == skillType)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }
        return null;
    }

    /// <summary>
    /// 更新血量UI
    /// </summary>
    private void UpdateHealthBar()
    {
        float currentHealth = Mathf.RoundToInt(player.health.GetCurrentHealth());
        float maxHealth = player.stats.GetMaxHealth();
        float sizeDiffrnece = Mathf.Abs(maxHealth - healthRect.sizeDelta.x);

        // 动态调整血条宽度
        if (sizeDiffrnece > 0.1f)
            healthRect.sizeDelta = new Vector2(maxHealth, healthRect.sizeDelta.y);

        healthText.text = $"{currentHealth}/{maxHealth}";
        healthSlider.value = player.health.GetHealthPercent();
    }
}