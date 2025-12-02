// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:36:20
// 版本：V1.1
// 描述：玩家状态抽象基类，统一管理所有状态的通用行为和属性
// ========================================================

using UnityEngine;

/// <summary>
/// 玩家状态抽象基类（所有具体状态如闲置、移动、攻击均继承此类）
/// </summary>
public abstract class EntityState
{
    #region 受保护字段（子类可访问）
    protected Player player; // 玩家实例引用
    protected StateMachine stateMachine; // 状态机管理器（用于切换状态）
    protected string animBoolName; // 动画控制器对应的布尔参数名

    protected Animator anim; // 动画组件（从Player获取）
    protected Rigidbody2D rb; // 2D刚体组件（从Player获取）
    protected PlayerInputSet input; // 玩家输入集合（从Player获取）

    protected float stateTimer; // 状态计时器（控制状态持续时间）
    protected bool triggerCalled; // 动画触发标记（避免重复攻击等行为）
    #endregion

    /// <summary>
    /// 构造函数：初始化核心依赖
    /// </summary>
    /// <param name="player">玩家实例</param>
    /// <param name="stateMachine">状态机</param>
    /// <param name="animBoolName">动画布尔参数名</param>
    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        // 一次获取常用组件，提升性能
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true); // 激活状态动画
        triggerCalled = false; // 重置触发标记
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // 计时器倒计时
        anim.SetFloat("yVelocity", rb.linearVelocity.y); // 传递Y轴速度给动画

        // 检测冲刺输入，满足条件则切换到冲刺状态
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false); // 关闭状态动画
    }

    /// <summary>
    /// 动画事件回调：标记触发已执行（避免重复行为）
    /// </summary>
    public void CallAnimtionTrigger()
    {
        triggerCalled = true;
    }

    /// <summary>
    /// 冲刺条件判定（贴墙/已在冲刺状态时不可冲刺）
    /// </summary>
    private bool CanDash()
    {
        if (player.wallDetected || stateMachine.currentState == player.dashState)
            return false;
        return true;
    }
}