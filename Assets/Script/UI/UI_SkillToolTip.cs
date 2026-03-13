// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 18:08:27
// 版本：V1.1
// 描述：UI技能提示工具
// ========================================================

using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTree;


    [SerializeField] private TextMeshProUGUI skillName;// 技能名称
    [SerializeField] private TextMeshProUGUI skillDescription;// 技能描述文本
    [SerializeField] private TextMeshProUGUI skillRequirements;// 技能解锁条件文本

    [Space]
    [SerializeField] private string metConditionHex;// 条件满足时的文本颜色
    [SerializeField] private string notMetConditionHex;// 条件未满足时的文本颜色
    [SerializeField] private string importantInfoHex;// 重要信息文本颜色
    [SerializeField] private Color exampleColor;// 示例颜色
    // 锁定技能的提示文本
    [SerializeField] private string lockedSkillText = "You've taken a diffrent path - this skill is now locked.";

    private Coroutine textEffectCo;// 文本闪烁特效的协程引用
    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }
    /// <summary>
    /// 基础显示逻辑（重写父类方法，无技能节点参数）
    /// </summary>
    public override void ShowToolTip(bool show,RectTransform targetRect)
    {
        base.ShowToolTip(show,targetRect);
    }
    /// <summary>
    /// 技能专属显示逻辑（核心重载方法）
    /// 根据技能节点数据填充提示框内容，按条件着色
    /// </summary>
    public void ShowToolTip(bool show,RectTransform targetRect,UI_TreeNode node)
    {
        // 调用父类方法，处理提示框的显示/隐藏和位置锚定
        base.ShowToolTip(show, targetRect);
        // 隐藏提示框时直接返回
        if (show == false)
            return;

        // 1. 设置技能名称和描述
        skillName.text = node.skillData.displayName;
        skillDescription.text = node.skillData.description;
        // 2. 处理锁定状态：锁定时显示锁定文本，否则显示解锁条件
        string skillLockedText = GetColoredText(importantInfoHex,lockedSkillText);
        string requirements = node.isLocked ? skillLockedText : GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);
        // 3. 设置解锁条件文本
        skillRequirements.text = requirements;
    }
    /// <summary>
    /// 触发锁定技能的文本闪烁特效
    /// 停止已有协程，重新启动闪烁逻辑
    /// </summary>
    public void LockedSkillEffect()
    {
        if(textEffectCo != null)
            StopCoroutine(textEffectCo);

        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirements,.15f,3));
    }
    /// <summary>
    /// 文本闪烁特效协程
    /// 交替切换锁定文本的颜色，实现闪烁效果
    /// </summary>
    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        // 循环指定次数的闪烁
        for (int i = 0; i < blinkCount; i++)
        {
            // 第一步：设置为未满足条件的颜色
            text.text = GetColoredText(notMetConditionHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
            // 第二步：设置为重要信息的颜色
            text.text = GetColoredText(importantInfoHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    /// <summary>
    /// 生成技能解锁条件文本（技能点+前置技能+冲突技能）
    /// </summary>
    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();
        // 1. 添加解锁条件标题
        sb.AppendLine("Requirements:");
        // 2. 处理技能点条件
        string costColor = skillTree.EnoughSkillPoints(skillCost) ? metConditionHex : notMetConditionHex;
        string costText = $"-{skillCost} skill point(s)";
        string finalCostText = GetColoredText(costColor, costText);

        sb.AppendLine(finalCostText);
        // 3. 处理前置技能节点
        foreach (var node in neededNodes)
        {
            if(node == null) continue;
            // 前置技能已解锁 → 满足颜色，否则未满足颜色
            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            string nodeText = $"-{node.skillData.displayName}";
            string finalNodeText = GetColoredText(nodeColor, nodeText);

            sb.AppendLine(finalNodeText);
        }
        // 4. 处理冲突技能节点
        if(conflictNodes.Length <= 0)
            return sb.ToString();
        // 5. 添加冲突技能标题
        sb.AppendLine();
        sb.AppendLine(GetColoredText(importantInfoHex,"Locks out: "));
        // 6. 遍历冲突技能节点，添加到文本

        foreach (var node in conflictNodes)
        {
            if(node == null) continue;  

            string nodeText = $"-{node.skillData.displayName}";
            string finalNodeText = GetColoredText(importantInfoHex, nodeText);
            sb.AppendLine(finalNodeText);
        }

        return sb.ToString();
    }
    

    

}
