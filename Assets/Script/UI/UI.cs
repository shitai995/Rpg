// ========================================================
// 作者：娇娇 
// 时间：2026-03-07
// 版本：V1.1
// 描述：UI核心管理器，统一管控所有界面、悬浮提示、界面切换与玩家输入锁定
// ========================================================

using UnityEngine;

/// <summary>
/// 全局UI总控制器
/// 统一管理背包、技能树、仓库、合成、商店、设置面板及各类悬浮提示
/// </summary>
public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput { get; private set; }
    private PlayerInputSet input;

    #region 提示框引用
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }
    #endregion

    #region 功能界面引用
    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_DeathScreen deathScreenUI { get; private set; }
    public UI_FadeScreen fadeScreenUI { get; private set; }
    #endregion

    // 界面开关状态
    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()
    {
        // 获取所有悬浮提示组件
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        // 获取所有功能界面（包含隐藏物体）
        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathScreenUI = GetComponentInChildren<UI_DeathScreen>(true);
        fadeScreenUI = GetComponentInChildren<UI_FadeScreen>(true);
        // 记录界面初始激活状态
        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    private void Start()
    {
        skillTreeUI.UnlockDefaultSkills();
    }

    /// <summary>
    /// 绑定输入事件，注册UI快捷键
    /// </summary>
    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        // 技能树开关
        input.UI.SkillTreeUI.performed += _ => ToggleSkillTreeUI();
        // 背包开关
        input.UI.InventoryUI.performed += _ => ToggleInventoryUI();

        // 备用输入按键状态
        input.UI.AlternativeInput.performed += _ => alternativeInput = true;
        input.UI.AlternativeInput.canceled += _ => alternativeInput = false;

        // 设置面板开关
        input.UI.OptionsUI.performed += _ =>
        {
            // 存在已打开界面则关闭并恢复游戏
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            // 无界面则暂停游戏并打开设置
            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }
    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        input.Disable(); // pay attention to this if you use gamepad
    }
    /// <summary>
    /// 打开设置面板
    /// </summary>
    public void OpenOptionsUI()
    {
        HideAllTooltips();
        StopPlayerControls(true);
        SwitchTo(optionsUI.gameObject);
    }

    /// <summary>
    /// 切回游戏主界面，关闭所有弹窗
    /// </summary>
    public void SwitchToInGameUI()
    {
        HideAllTooltips();
        StopPlayerControls(false);
        SwitchTo(inGameUI.gameObject);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }

    /// <summary>
    /// 关闭所有UI，仅激活指定界面
    /// </summary>
    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach (var element in uiElements)
            element.SetActive(false);

        objectToSwitchOn.SetActive(true);
    }

    /// <summary>
    /// 启用/禁用玩家移动、技能等操控
    /// </summary>
    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }

    /// <summary>
    /// 根据当前界面状态，判断是否需要锁定玩家操控
    /// </summary>
    private void StopPlayerControlsIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }
        StopPlayerControls(false);
    }

    /// <summary>
    /// 切换技能树界面显隐
    /// </summary>
    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();
        fadeScreenUI.transform.SetAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllTooltips();

        StopPlayerControlsIfNeeded();
    }

    /// <summary>
    /// 切换背包界面显隐
    /// </summary>
    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();
        fadeScreenUI.transform.SetAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        HideAllTooltips();

        StopPlayerControlsIfNeeded();
    }

    /// <summary>
    /// 开启/关闭仓库界面
    /// </summary>
    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControls(openStorageUI);

        // 关闭仓库时同步关闭合成界面与提示
        if (!openStorageUI)
        {
            craftUI.gameObject.SetActive(false);
            HideAllTooltips();
        }
    }

    /// <summary>
    /// 开启/关闭商店界面
    /// </summary>
    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControls(openMerchantUI);

        if (!openMerchantUI)
            HideAllTooltips();
    }

    /// <summary>
    /// 关闭所有悬浮提示（物品/技能/属性）
    /// </summary>
    public void HideAllTooltips()
    {
        itemToolTip.ShowToolTip(false, null);
        skillToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }

    /// <summary>
    /// 把所有提示框层级置到最顶层
    /// </summary>
    private void SetTooltipsAsLastSibling()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }
}