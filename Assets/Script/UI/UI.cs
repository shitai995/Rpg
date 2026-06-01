// ========================================================
// 作者：娇娇 
// 时间：2026-03-07
// 版本：V1.1
// 描述：UI核心管理器（统一管理所有界面、提示框、开关逻辑）
// ========================================================

using UnityEngine;

/// <summary>
/// 全局UI总控制器
/// 负责管理所有子界面：背包、技能树、储物、合成、商店、提示框
/// </summary>
public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput {  get; private set; }
    private PlayerInputSet input;
    #region UI
    // 各类提示框
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    // 功能界面
    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_InGame inGameUI {  get; private set; }
    public UI_Options optionsUI { get; private set; }
    #endregion
    // 界面开关状态
    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()
    {
        // 获取所有提示框
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        // 获取所有功能界面（包含隐藏对象）
        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);

        // 记录初始激活状态
        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }
    public void Start()
    {
        skillTreeUI.UnlockDefaultSkills();
    }
    public void SetupControlsUI(PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.SkillTreeUI.performed += ctx => ToggleSkillTreeUI();
        input.UI.InventoryUI.performed += ctx => ToggleInventoryUI();

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.OptionsUI.performed += ctx =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionsUI();
        };
    }
    public void OpenOptionsUI()
    {
        HideAllTooltips();
        StopPlayerControls(true);
        SwitchTo(optionsUI.gameObject);

    }
    public void SwitchToInGameUI()
    {
        HideAllTooltips();
        StopPlayerControls(false);
        SwitchTo(optionsUI.gameObject);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }
    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        objectToSwitchOn.SetActive(true);
    }
    private void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }
    private void StopPlayerControlsIfNeeded()
    {
        foreach(var element in uiElements)
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
    /// 开关技能树界面
    /// </summary>
    public void ToggleSkillTreeUI()
    {
        skillTreeUI.transform.SetAsLastSibling();
        SetToolTipsAsLastSibling();

        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        HideAllTooltips();

        StopPlayerControlsIfNeeded();
    }

    /// <summary>
    /// 开关背包界面
    /// </summary>
    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetToolTipsAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);

        // 关闭所有悬浮提示
        HideAllTooltips();

        StopPlayerControlsIfNeeded();
    }
    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);  
        StopPlayerControls(openStorageUI);
        if(openStorageUI == false)
        {
            craftUI.gameObject.SetActive(false);    
            HideAllTooltips();
        }
    }

    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControls(merchantUI);

        if (openMerchantUI == false)
            HideAllTooltips();
    }
    /// <summary>
    /// 强制关闭所有悬浮提示（物品/技能/属性）
    /// </summary>
    public void HideAllTooltips()
    {
        itemToolTip.ShowToolTip(false, null);
        skillToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }
    private void SetToolTipsAsLastSibling()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }

}