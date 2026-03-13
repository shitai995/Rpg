// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 15:58:38
// 版本：V1.1
// 描述：UI核心管理器
// ========================================================

using System.Security.Cryptography;
using UnityEngine;

public class UI : MonoBehaviour
{

    public UI_SkillToolTip skillToolTip;// 技能提示框组件
    public UI_SkillTree skillTree;// 技能树UI组件
    private bool skillTreeEnabled;// 技能树UI的启用状态

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
    }
    /// <summary>
    /// 切换技能树UI的显示/隐藏状态
    /// 外部调用入口（如玩家按下技能树按键、点击按钮）
    /// </summary>
    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTree.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
    }
}
