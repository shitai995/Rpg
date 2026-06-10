// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-11 22:24:01
// 版本：V1.1
// 描述：所有游戏实体基类，封装通用组件、移动、碰撞、击退、翻转等逻辑
// ========================================================

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 实体基类，角色、敌人等实体的通用父类
/// </summary>
public class Entity : MonoBehaviour
{
    // 角色翻转事件
    public event Action OnFlipped;

    #region 核心组件
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Entity_SFX sfx {  get; private set; }
    protected StateMachine stateMachine;
    #endregion

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1; // 1朝右，-1朝左

    [Header("碰撞检测")]
    public LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;

    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool isKnocked;               // 击退状态标记
    private Coroutine knockbackCo;        // 击退协程
    private Coroutine slowDownCo;         // 减速协程

    /// <summary>
    /// 初始化组件与状态机
    /// </summary>
    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<Entity_SFX>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    /// <summary>
    /// 帧更新：碰撞检测 + 状态机逻辑
    /// </summary>
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    /// <summary>
    /// 动画事件回调，触发状态动画动作
    /// </summary>
    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimtionTrigger();
    }

    /// <summary>
    /// 实体死亡逻辑
    /// </summary>
    public virtual void EntityDeath()
    {

    }

    /// <summary>
    /// 施加减速效果
    /// </summary>
    public virtual void SlowDownEntity(float duration, float slowMultiplier, bool canOverrideSlowEffect = false)
    {
        if (slowDownCo != null)
        {
            if (canOverrideSlowEffect)
                StopCoroutine(slowDownCo);
            else
                return;
        }
        slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    /// <summary>
    /// 减速协程（由子类重写）
    /// </summary>
    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    /// <summary>
    /// 停止减速
    /// </summary>
    public virtual void StopSlowDown()
    {
        slowDownCo = null;
    }

    /// <summary>
    /// 施加击退效果
    /// </summary>
    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);
        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    /// <summary>
    /// 击退协程
    /// </summary>
    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;
        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    /// <summary>
    /// 设置刚体速度并处理朝向
    /// </summary>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    /// <summary>
    /// 根据移动方向判断是否翻转
    /// </summary>
    public void HandleFlip(float xVelcoity)
    {
        if (xVelcoity > 0 && !facingRight)
            Flip();
        else if (xVelcoity < 0 && facingRight)
            Flip();
    }

    /// <summary>
    /// 翻转角色朝向
    /// </summary>
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
        OnFlipped?.Invoke();
    }

    /// <summary>
    /// 地面、墙壁射线检测
    /// </summary>
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
    }

    /// <summary>
    /// 绘制检测辅助线
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}