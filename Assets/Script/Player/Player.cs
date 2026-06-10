// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:37:34
// 版本：V1.1
// 描述：玩家核心控制类，整合输入、状态机、物理和动画逻辑
// ========================================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家控制器：统一管理输入、状态机、刚体和动画，协调各模块交互
/// </summary>
public class Player : Entity
{
    public static Player instance;
    public static event Action OnPlayerDeath;

    public UI ui {  get; private set; }
    public PlayerInputSet input { get; private set; } // 输入集合
    public Player_SkillManager skillManager { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }
    public Player_Combat combat { get; private set; }
    public Inventory_Player inventory { get; private set; }
    public Player_Stats stats { get; private set; }

    #region State Varisbles
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
    public Player_SwordThrowState swordThrowState {  get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }
    #endregion
    #region 配置参数（Inspector面板设置）
    [Header("攻击相关配置")]
    public Vector2[] attackVelocity; // 攻击时移动速度数组（对应多段攻击）
    public Vector2 jumpAttackVelocity; // 跳跃攻击移动速度
    public float jumpCutMultiplier = .4f;// 松手后保留多少上升速度
    [Header("跳跃手感优化")]
    public float coyoteTime = 0.12f; // 离开平台后仍可跳跃的容错时间
    public float jumpBufferTime = 0.1f; // 落地前按跳跃键的缓冲时间
    public float wallJumpWallDetectDelay = 0.2f; // 蹬墙跳后忽略墙壁检测的时长
    [HideInInspector] public float lastGroundedTime; // 最后一次在地面的时间
    [HideInInspector] public float lastJumpPressTime; // 最后一次按跳跃键的时间
    [HideInInspector] public float lastWallJumpTime; // 最后一次蹬墙跳的时间
    public float attackVelocityDuration = .1f; // 攻击速度持续时间
    public float comboResetTime = 1; // 连招重置时间
    public Coroutine queuedAttackCo; // 延迟攻击协程引用

    [Header("Ultimate ability details")]
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;


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
    public Vector2 mousePosition {  get; private set; }// 鼠标位置

    // 输入回调委托（用于 OnEnable/OnDisable 订阅和取消订阅）
    private System.Action<InputAction.CallbackContext> onMousePerformed;
    private System.Action<InputAction.CallbackContext> onMovementPerformed;
    private System.Action<InputAction.CallbackContext> onMovementCanceled;
    private System.Action<InputAction.CallbackContext> onSpellPerformed1;
    private System.Action<InputAction.CallbackContext> onSpellPerformed2;
    private System.Action<InputAction.CallbackContext> onInteractPerformed;
    private System.Action<InputAction.CallbackContext> onQuickSlot1Performed;
    private System.Action<InputAction.CallbackContext> onQuickSlot2Performed;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        instance = this;

        ui = FindAnyObjectByType<UI>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        skillManager = GetComponent<Player_SkillManager>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        combat = GetComponent<Player_Combat>();
        inventory = GetComponent<Inventory_Player>();
        stats = GetComponent<Player_Stats>();

        input = new PlayerInputSet();
        ui.SetupControlsUI(input);

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
        swordThrowState = new Player_SwordThrowState(this, stateMachine, "swordThrow");
        domainExpansionState = new Player_DomainExpansionState(this, stateMachine, "jumpFall");

    }
    /// <summary>
    /// 初始化玩家为待机状态
    /// </summary>
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
    // 玩家瞬移方法
    public void TeleportPlayer(Vector3 position) => transform.position = position;

    /// <summary>
    /// 实体减速协程
    /// 临时降低玩家移动、跳跃、攻击等速度，持续指定时长后恢复原值
    /// </summary>
    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = (Vector2[])attackVelocity.Clone();
        // 计算实际减速倍率（1 - 传入的减速比例）
        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier; 
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        wallJumpForce = wallJumpForce * speedMultiplier;
        jumpAttackVelocity = jumpAttackVelocity * speedMultiplier;
        // 遍历攻击速度数组，逐元素应用减速
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }
        // 等待减速持续时长
        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;
        // 恢复攻击速度数组
        for ( int i = 0;i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
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
    private void TryInteract()
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Collider2D[] objectsAround = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (var target in objectsAround)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable == null) continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }

        if (closest == null)
            return;

        closest.GetComponent<IInteractable>().Interact();
    }
    /// <summary>
    /// 启用输入并绑定移动输入事件
    /// </summary>
    private void OnEnable()
    {
        input.Enable();

        onMousePerformed = ctx => mousePosition = ctx.ReadValue<Vector2>();
        onMovementPerformed = ctx => moveInput = ctx.ReadValue<Vector2>();
        onMovementCanceled = ctx => moveInput = Vector2.zero;
        onSpellPerformed1 = ctx => skillManager.timeEcho.TryUseSkill();
        onSpellPerformed2 = ctx => skillManager.shard.TryUseSkill();
        onInteractPerformed = ctx => TryInteract();
        onQuickSlot1Performed = ctx => inventory.TryUseQuickItemInSlot(1);
        onQuickSlot2Performed = ctx => inventory.TryUseQuickItemInSlot(2);

        input.Player.Mouse.performed += onMousePerformed;
        input.Player.Movement.performed += onMovementPerformed;
        input.Player.Movement.canceled += onMovementCanceled;
        input.Player.Spell.performed += onSpellPerformed1;
        input.Player.Spell.performed += onSpellPerformed2;
        input.Player.Interact.performed += onInteractPerformed;
        input.Player.QuickItemSlot_1.performed += onQuickSlot1Performed;
        input.Player.QuickItemSlot_2.performed += onQuickSlot2Performed;
    }

    private void OnDisable()
    {
        input.Player.Mouse.performed -= onMousePerformed;
        input.Player.Movement.performed -= onMovementPerformed;
        input.Player.Movement.canceled -= onMovementCanceled;
        input.Player.Spell.performed -= onSpellPerformed1;
        input.Player.Spell.performed -= onSpellPerformed2;
        input.Player.Interact.performed -= onInteractPerformed;
        input.Player.QuickItemSlot_1.performed -= onQuickSlot1Performed;
        input.Player.QuickItemSlot_2.performed -= onQuickSlot2Performed;

        input.Disable();
    }
}