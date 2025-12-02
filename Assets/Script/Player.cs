// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:37:34
// 版本：V1.1
// 描述：玩家核心控制类，整合输入、状态机、物理和动画逻辑
// ========================================================

using System.Collections;
using UnityEngine;

/// <summary>
/// 玩家控制器：统一管理输入、状态机、刚体和动画，协调各模块交互
/// </summary>
public class Player : MonoBehaviour
{
    #region 组件与核心对象（外部只读，内部赋值）
    public Animator anim { get; private set; } // 动画组件
    public Rigidbody2D rb { get; private set; } // 2D刚体组件
    public PlayerInputSet input { get; private set; } // 输入集合
    private StateMachine stateMachine; // 状态机管理器


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
    #endregion

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

    [Header("碰撞检测")]
    [SerializeField] private float groundCheckDistance; // 地面检测射线长度
    [SerializeField] private float wallCheckDistance; // 墙壁检测射线长度
    [SerializeField] private LayerMask whatIsGround; // 地面层掩码
    [SerializeField] private Transform primaryWallCheck; // 主墙壁检测点
    [SerializeField] private Transform secondaryWallCheck; // 副墙壁检测点
    #endregion

    #region 状态变量（外部可访问）
    public bool facingRight = true; // 是否面朝右
    public int facingDir { get; private set; } = 1; // 面向方向（1=右，-1=左）
    public Vector2 moveInput { get; private set; } // 移动输入值
    public bool groundDetected { get; private set; } // 是否检测到地面
    public bool wallDetected { get; private set; } // 是否检测到墙壁
    #endregion

    /// <summary>
    /// 初始化组件和状态机
    /// </summary>
    private void Awake()
    {
        // 获取核心组件
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
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

    /// <summary>
    /// 设置状态机初始状态为闲置
    /// </summary>
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    /// <summary>
    /// 每帧更新：碰撞检测 + 状态机更新
    /// </summary>
    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
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
    /// 动画事件回调：通知当前状态触发已执行（避免重复攻击）
    /// </summary>
    public void CallAnimtionTrigger()
    {
        stateMachine.currentState.CallAnimtionTrigger();
    }

    /// <summary>
    /// 设置刚体速度（含翻转处理）
    /// </summary>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    /// <summary>
    /// 根据X轴速度处理角色翻转
    /// </summary>
    private void HandleFlip(float xVelcoity)
    {
        if (xVelcoity > 0 && !facingRight)
            Flip();
        else if (xVelcoity < 0 && facingRight)
            Flip();
    }

    /// <summary>
    /// 角色翻转（切换面向方向）
    /// </summary>
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    /// <summary>
    /// 碰撞检测：地面+墙壁（射线检测）
    /// </summary>
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        // 双检测点确认墙壁（避免误判）
        wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    /// <summary>
    /// Gizmos辅助线：场景视图显示检测射线（便于调试）
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}