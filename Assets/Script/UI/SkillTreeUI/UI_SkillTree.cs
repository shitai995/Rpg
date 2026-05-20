// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-08 15:25:53
// 版本：V1.1
// 描述：技能树UI核心管理器
// 负责技能树的核心逻辑：技能点管理、节点连接更新、技能重置（退款）
// 是技能树UI与玩家技能系统（Player_SkillManager）的核心桥梁
// ========================================================

using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;// 玩家当前拥有的技能点
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;// 父节点连接处理器数组
    public Player_SkillManager skillManager { get; private set; }

    private void Awake()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>(); 
    }

    private void Start()
    {
        UpdateAllConnections();// 启动时：更新所有技能节点的连接状态
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
    public void RemoveSkillPoints(int cost) => skillPoints = skillPoints - cost;
    /// <summary>
    /// 增加技能点
    /// </summary>
    public void AddSkillPoints(int points) => skillPoints = skillPoints + points;
    


    [ContextMenu("节点连接更新")]

    public void UpdateAllConnections()
    {
        // 遍历所有父节点连接处理器，更新连接线
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

}
