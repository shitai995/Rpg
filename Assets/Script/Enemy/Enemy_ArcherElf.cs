// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 12:59:06
// 版本：V1.1
// 描述：精灵弓箭手敌人主体类，包含射箭、反制眩晕、状态初始化逻辑
// ========================================================

using UnityEngine;

/// <summary>精灵弓箭手敌人主逻辑</summary>
public class Enemy_ArcherElf : Enemy
{
    // 是否可被玩家反击眩晕
    public bool CanBeCountered => canBeStunned;
    // 专属战斗状态对外引用
    public Enemy_ArcherElfBattleState elftBattleState { get; set; }

    [Header("精灵弓箭手专属配置")]
    [SerializeField] private GameObject arrowPrefab;    // 箭矢预制体
    [SerializeField] private Transform arrowStartPoint; // 箭矢生成挂载点
    [SerializeField] private float arrowSpeed = 8;      // 箭矢飞行速度

    protected override void Awake()
    {
        base.Awake();
        // 初始化基础状态
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        // 初始化弓箭手专属战斗状态，替换父类战斗状态
        elftBattleState = new Enemy_ArcherElfBattleState(this, stateMachine, "battle");
        battleState = elftBattleState;
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState); // 初始进入闲置状态
    }

    /// <summary>射箭特殊攻击，生成箭矢并初始化速度与伤害数据</summary>
    public override void SpecialAttack()
    {
        GameObject newArrow = Instantiate(arrowPrefab, arrowStartPoint.position, Quaternion.identity);
        newArrow.GetComponent<Enemy_ArcherElfArrow>().SetupArrow(arrowSpeed * facingDir, combat);
    }

    /// <summary>玩家反击触发，进入眩晕状态</summary>
    public void HandleCounter()
    {
        if (!CanBeCountered)
            return;
        stateMachine.ChangeState(stunnedState);
    }
}