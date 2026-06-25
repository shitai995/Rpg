// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-08 15:25:53
// 版本：V1.1
// 描述：技能树UI核心管理器
// 负责技能树的核心逻辑：技能点管理、节点连接更新、技能重置（退款）
// 是技能树UI与玩家技能系统（Player_SkillManager）的核心桥梁
// ========================================================

using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// 技能树界面管理器，实现存档读写、技能点增减、节点状态刷新
/// </summary>
public class UI_SkillTree : MonoBehaviour, ISaveable
{
    [SerializeField] private int skillPoints;                     // 当前可用技能点
    [SerializeField] private TextMeshProUGUI skillPointsText;      // 技能点显示文本
    [SerializeField] private UI_TreeConnectHandler[] parentNodes; // 节点连线处理器数组

    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }

    private void Start()
    {
        UpdateAllConnections();
        UpdateSkillPointsUI();
    }

    /// <summary>
    /// 刷新技能点文本显示
    /// </summary>
    private void UpdateSkillPointsUI()
    {
        skillPointsText.text = skillPoints.ToString();
    }

    /// <summary>
    /// 初始化并解锁默认技能
    /// </summary>
    public void UnlockDefaultSkills()
    {
        if (allTreeNodes == null)
            allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);

        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach (var node in allTreeNodes)
            node.UnlockDefaultSkill();
    }

    /// <summary>
    /// 重置所有技能并返还技能点（编辑器右键菜单）
    /// </summary>
    [ContextMenu("重置所有技能")]
    public void RefundAllskills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();
        foreach (var node in skillNodes)
            node.Refund();
    }

    /// <summary>
    /// 判断技能点是否充足
    /// </summary>
    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;

    /// <summary>
    /// 扣除技能点
    /// </summary>
    public void RemoveSkillPoints(int cost)
    {
        skillPoints -= cost;
        UpdateSkillPointsUI();
    }

    /// <summary>
    /// 增加技能点
    /// </summary>
    public void AddSkillPoints(int points)
    {
        skillPoints += points;
        UpdateSkillPointsUI();
    }

    /// <summary>
    /// 全局更新所有技能节点连线状态（编辑器右键菜单）
    /// </summary>
    [ContextMenu("节点连接更新")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
            node.UpdateAllConnections();
    }

    /// <summary>
    /// 读取存档数据，恢复技能点与技能解锁状态
    /// </summary>
    public void LoadData(GameData data)
    {
        // GameObject 未激活时 Awake 不会执行，需要就地初始化
        if (allTreeNodes == null)
            allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);

        skillPoints = data.skillPoints;

        // 恢复技能节点解锁状态
        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;
            if (data.skillTreeUI.TryGetValue(skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        // 恢复技能进阶形态
        if (skillManager == null) return;
        foreach (var skill in skillManager.allSkills)
        {
            if (data.skillUpgrades.TryGetValue(skill.GetSkillType(), out SkillUpgradeType upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(n => n.skillData.upgradeData.upgradeType == upgradeType);
                //upgradeNode?.skillData?.SetSkillUpgrade(skill);
            }
        }
    }

    /// <summary>
    /// 保存当前技能树数据至存档
    /// </summary>
    public void SaveData(ref GameData data)
    {
        if (allTreeNodes == null)
            allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);

        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        // 记录所有节点解锁状态
        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }

        // 记录所有技能进阶形态
        if (skillManager == null) return;
        foreach (var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgrade();
        }
    }
}