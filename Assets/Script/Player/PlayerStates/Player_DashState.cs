// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-30 20:37:08
// 版本：V1.1
// 描述：玩家冲刺状态
// 核心逻辑：冲刺期间零重力、固定冲刺速度，支持墙面碰撞中断、落地/空中结束后切换对应状态
// ========================================================

using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale; // 原始重力缩放（用于冲刺结束后恢复）
    private int dashDir; // 冲刺方向（跟随输入或当前面向）

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter(); // 激活冲刺动画、重置触发标记

        skillManager.dash.OnStartEffect();
        player.vfx.DoImageEchoEffect(player.dashDuration);

        // 确定冲刺方向：有水平输入则跟随输入，无则沿用当前面向
        dashDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;
        // 初始化冲刺计时器（控制冲刺持续时间）
        stateTimer = player.dashDuration;

        // 保存原始重力并设为0（冲刺期间无重力，保持水平冲刺）
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
        player.gameObject.layer = LayerMask.NameToLayer("Untargetable");
    }

    public override void Update()
    {
        base.Update(); // 保留基类计时器倒计时、冲刺检测等逻辑

        CancelDashIfNeeded(); // 检测墙面碰撞，必要时中断冲刺
        // 应用冲刺速度：水平方向为冲刺速度×方向，竖直方向设0（零重力）
        player.SetVelocity(player.dashSpeed * dashDir, 0);

        // 冲刺计时器结束：根据是否在地面切换状态
        if (stateTimer < 0)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit(); // 关闭冲刺动画

        skillManager.dash.OnEndEffect();

        player.health.SetCanTakeDamage(true);
        player.SetVelocity(0, 0); // 重置速度（避免冲刺后残留速度）
        rb.gravityScale = originalGravityScale; // 恢复原始重力
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// 冲刺中断检测：冲刺时撞墙则切换对应状态
    /// </summary>
    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            // 撞墙且在地面→闲置状态；撞墙且在空中→滑墙状态
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}