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
public abstract class PlayerState : EntityState
{

    protected Player player; // 玩家实例引用
    protected PlayerInputSet input; // 玩家输入集合（从Player获取）  

    /// <summary>
    /// 构造函数：初始化核心依赖
    /// </summary>
    /// <param name="player">玩家实例</param>
    /// <param name="stateMachine">状态机</param>
    /// <param name="animBoolName">动画布尔参数名</param>
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine,animBoolName)
    {
        this.player = player;
     
        // 一次获取常用组件，提升性能
        anim = player.anim;
        rb = player.rb;
        input = player.input;
        stats = player.stats;
    }

    public override void Update()
    {
        base.Update();


        // 检测冲刺输入，满足条件则切换到冲刺状态
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }
    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        anim.SetFloat("yVelocity", rb.linearVelocity.y); // 传递Y轴速度给动画
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