// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:31:03
// 版本：V1.1
// 描述：敌人主体逻辑类，管理状态、属性、检测与行为
// ========================================================

using System.Collections;
using UnityEngine;

/// <summary>
/// 敌人基类，继承自实体基类，控制敌人所有行为与状态
/// </summary>
public class Enemy : Entity
{
    [Header("Quest Info")]
    public string questTargetId;

    public Entity_Stats stats { get; private set; }
    public Enemy_Health health { get; private set; }

    // 敌人所有状态
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("战斗配置")]
    public float battleMoveSpeed = 3;        // 战斗移动速度
    public float attackDistance = 2;         // 攻击判定距离
    public float battleTimeDuration = 5;      // 战斗状态持续时长
    public float minRetreatDistance = 1;      // 攻击后最小后撤距离
    public Vector2 retreatVelocity;           // 后撤速度

    [Header("反击设置")]
    public float stunnedDuration = 1;         // 受击僵直时长
    public Vector2 stunnedVelocity = new Vector2(7, 7); // 僵直冲量
    [SerializeField] protected bool canBeStunned; // 是否可被僵直

    [Header("移动配置")]
    public float idleTime = 2f;               // 原地待机时长
    public float moveSpeed = 1.4f;            // 常规移动速度
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;  // 移动动画速率缩放

    [Header("玩家检测")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;  // 检测点挂载节点
    [SerializeField] private float playerCheckDistance = 10; // 检测范围
    public Transform player { get; private set; }

    public float activeSlowMultiplier { get; private set; } = 1; // 减速倍率

    // 获取最终移动速度
    public float GetMoveSpeed() => moveSpeed * activeSlowMultiplier;
    public float GetBattleMoveSpeed() => battleMoveSpeed * activeSlowMultiplier;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Enemy_Health>();
        stats = GetComponent<Entity_Stats>();
    }

    /// <summary>
    /// 实体减速协程
    /// </summary>
    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        activeSlowMultiplier = 1 - slowMultiplier;
        anim.speed *= activeSlowMultiplier;
        yield return new WaitForSeconds(duration);
        StopSlowDown();
    }

    /// <summary>
    /// 解除减速
    /// </summary>
    public override void StopSlowDown()
    {
        activeSlowMultiplier = 1;
        anim.speed = 1;
        base.StopSlowDown();
    }

    /// <summary>
    /// 开关僵直判定
    /// </summary>
    public void EnableCounterWindow(bool enabled) => canBeStunned = enabled;

    /// <summary>
    /// 敌人死亡逻辑
    /// </summary>
    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
    }

    /// <summary>
    /// 玩家死亡，敌人切回待机
    /// </summary>
    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    /// <summary>
    /// 进入战斗状态
    /// </summary>
    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;

        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    /// <summary>
    /// 获取玩家引用
    /// </summary>
    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;
        return player;
    }

    /// <summary>
    /// 射线检测玩家
    /// </summary>
    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position,
            Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    /// <summary>
    /// 场景视图绘制辅助线
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 玩家检测范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position,
            new Vector3(playerCheck.position.x + facingDir * playerCheckDistance, playerCheck.position.y));

        // 攻击距离
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position,
            new Vector3(playerCheck.position.x + facingDir * attackDistance, playerCheck.position.y));

        // 后撤距离
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position,
            new Vector3(playerCheck.position.x + facingDir * minRetreatDistance, playerCheck.position.y));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}