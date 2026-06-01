// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-11 22:24:01
// 版本：V1.1
// 描述：实体类
// ========================================================

using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;



    #region 组件与核心对象（外部只读，内部赋值）
    public Animator anim { get; private set; } // 动画组件
    public Rigidbody2D rb { get; private set; } // 2D刚体组件
    protected StateMachine stateMachine; // 状态机管理器
    #endregion
    private bool facingRight = true; // 是否面朝右
    public int facingDir { get; private set; } = 1; // 面向方向（1=右，-1=左）

    [Header("碰撞检测")]
    public LayerMask whatIsGround; // 地面层掩码
    [SerializeField] private float groundCheckDistance; // 地面检测射线长度
    [SerializeField] private float wallCheckDistance; // 墙壁检测射线长度
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck; // 主墙壁检测点
    [SerializeField] private Transform secondaryWallCheck; // 副墙壁检测点
    
    
    public bool groundDetected { get; private set; } // 是否检测到地面
    public bool wallDetected { get; private set; } // 是否检测到墙壁


    private bool isKnocked;// 击退状态标记（击退期间禁用移动等逻辑）
    private Coroutine knockbackCo;// 击退协程引用（用于中断重复击退）
    private Coroutine slowDownCo;
    /// <summary>
    /// 初始化组件和状态机
    /// </summary>
    protected virtual void Awake()
    {
        // 获取核心组件
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
        
    }

    protected virtual void Start()
    {

    }

    /// <summary>
    /// 每帧更新：碰撞检测 + 状态机更新
    /// </summary>
    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }
    /// <summary>
    /// 当前状态触发器
    /// 动画事件回调：通知当前状态触发已执行（避免重复攻击）
    /// </summary>
    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimtionTrigger();
    }


    public virtual void EntityDeath()
    {

    }

    public virtual void SlowDownEntity(float duration, float slowMultiplier,bool canOverrideSlowEffect = false)
    {
        if(slowDownCo != null)
        {
            if (canOverrideSlowEffect)
                StopCoroutine(slowDownCo);
            else
                return;
        }

        slowDownCo = StartCoroutine(SlowDownEntityCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }
    public virtual void StopSlowDown()
    {
        slowDownCo = null;
    }
    /// <summary>
    /// 接收击退指令（对外公开接口）
    /// 特性：重复调用时中断上一次击退，重新执行
    /// </summary>
    public void ReciveKnockback(Vector2 knockback,float duration)
    {
        if(knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockback,duration));
    }
    /// <summary>
    /// 击退协程（内部执行逻辑）
    /// 流程：标记击退状态 → 施加击退速度 → 等待持续时间 → 重置速度和状态
    /// </summary>
    private IEnumerator KnockbackCo(Vector2 knockback,float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;

    }
    
    /// <summary>
    /// 设置刚体速度（含翻转处理）
    /// </summary>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    /// <summary>
    /// 根据X轴速度处理角色翻转
    /// </summary>
    public void HandleFlip(float xVelcoity)
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

        OnFlipped?.Invoke();
    }

    /// <summary>
    /// 碰撞检测：地面+墙壁（射线检测）
    /// </summary>
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        // 双检测点确认墙壁（避免误判）

        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    /// <summary>
    /// Gizmos辅助线：场景视图显示检测射线（便于调试）
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        if (secondaryWallCheck != null)
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
