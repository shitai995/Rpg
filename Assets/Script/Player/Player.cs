// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:37:34
// 版本：V1.1
// 描述：玩家核心控制类，整合输入、状态机、物理和动画逻辑
// ========================================================

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 玩家控制器：统一管理输入、状态机、刚体和动画，协调各模块交互
/// </summary>
public class Player : Entity
{
    public static event Action OnPlayerDeath;

    public PlayerInputSet input { get; private set; } // 输入集合
    // 所有玩家状态实例（供状态切换使用）
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }   

    #region 配置参数（Inspector面板设置）
    [Header("攻击相关配置")]
    public Vector2[] attackVelocity; // 攻击时移动速度数组（对应多段攻击）
    public Vector2 jumpAttackVelocity; // 跳跃攻击移动速度
    public float attackVelocityDuration = .1f; // 攻击速度持续时间
    public float comboResetTime = 1; // 连招重置时间
    public Coroutine queuedAttackCo; // 延迟攻击协程引用

    [Header("移动相关配置")]
    public float moveSpeed; // 地面移动速度
    public float jumpForce = 5; // 跳跃力
    public Vector2 wallJumpForce; // 墙跳力
    [Range(0f, 1f)]
    public float inAirMoveMultiplier = .7f; // 空中移动速度倍率
    [Range(0f, 1f)]
    public float wallSlideSlowMultiplier = .7f; // 滑墙减速倍率
    [Space]
    public float dashDuration = .25f; // 冲刺持续时间
    public float dashSpeed = 20; // 冲刺速度
    public Vector2 moveInput { get; private set; } // 移动输入值
    #endregion

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        // 初始化所有状态实例
        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
        counterAttackState = new Player_CounterAttackState(this,stateMachine, "counterAttack"); 
    }
    /// <summary>
    /// 初始化玩家为待机状态
    /// </summary>
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }
    /// <summary>
    /// 外部调用：延迟进入攻击状态（用于连招）
    /// </summary>
    public void EnterAtackStateWithDelay()
    {
        if (queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);
        queuedAttackCo = StartCoroutine(EnterAttackStartWithDelayCo());
    }

    /// <summary>
    /// 延迟攻击协程：帧末切换攻击状态（避免输入冲突）
    /// </summary>
    private IEnumerator EnterAttackStartWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    /// <summary>
    /// 启用输入并绑定移动输入事件
    /// </summary>
    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    /// <summary>
    /// 禁用输入
    /// </summary>
    private void OnDisable()
    {
        input.Disable();
    }

    
}