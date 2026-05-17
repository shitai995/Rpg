// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 15:58:38
// 版本：V1.1
// 描述：UI核心管理器（统一管理所有界面与提示框）
// ========================================================

using UnityEngine;
public class UI : MonoBehaviour
{
    // 各类提示框
    public UI_SkillToolTip skillToolTip { get; private set; }
    public Ui_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    // 功能界面
    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }

    private bool skillTreeEnabled;   // 技能树显示状态
    private bool inventoryEnabled;   // 背包显示状态

    private void Awake()
    {
        // 获取所有子物体中的UI组件
        itemToolTip = GetComponentInChildren<Ui_ItemToolTip>();
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);

        // 记录初始显示状态
        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    /// <summary>
    /// 切换技能树界面显示/隐藏，并关闭提示框
    /// </summary>
    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }

    /// <summary>
    /// 切换背包界面显示/隐藏，并关闭所有提示框
    /// </summary>
    public void ToggleInventoryUI()
    {
        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        statToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
    }
}