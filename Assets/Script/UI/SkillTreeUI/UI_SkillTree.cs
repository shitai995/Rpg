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

public class UI_SkillTree : MonoBehaviour,ISaveable
{
    [SerializeField] private int skillPoints;// 玩家当前拥有的技能点
    [SerializeField] private TextMeshProUGUI skillPointsText;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;// 父节点连接处理器数组
    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }

    private void Start()
    {
        UpdateAllConnections();// 启动时：更新所有技能节点的连接状态
        UpdateSkillPointsUI();
    }

    private void UpdateSkillPointsUI()
    {
        skillPointsText.text = skillPoints.ToString();
    }

    public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach(var node in allTreeNodes)
            node.UnlockDefaultSkill();
    }
    [ContextMenu("重置所有技能")]
    public void RefundAllskills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)// 遍历所有节点，执行退款逻辑
            node.Refund();
    }
    /// <summary>
    /// 检查技能点是否足够解锁指定技能
    /// </summary>
    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
    /// <summary>
    /// 消耗技能点
    /// </summary>
    public void RemoveSkillPoints(int cost)
    {
        skillPoints = skillPoints - cost;
        UpdateSkillPointsUI();
    }
    /// <summary>
    /// 增加技能点
    /// </summary>
    public void AddSkillPoints(int points)
    {
        skillPoints = skillPoints + points;
        UpdateSkillPointsUI();
    }

    [ContextMenu("节点连接更新")]
    public void UpdateAllConnections()
    {
        // 遍历所有父节点连接处理器，更新连接线
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    public void LoadData(GameData data)
    {
        skillPoints = data.skillPoints;

        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;

            if (data.skillTreeUI.TryGetValue(skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        foreach (var skill in skillManager.allSkills)
        {
            if (data.skillUpgrades.TryGetValue(skill.GetSkillType(), out SkillUpgradeType upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(node => node.skillData.upgradeData.upgradeType == upgradeType);

                if (upgradeNode != null)
                    skill.SetSkillUpgrade(upgradeNode.skillData);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        foreach (var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }


        foreach (var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgrade();
        }
    }
}
