// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-30 17:45:04
// 版本：V1.1
// 描述：玩家爬墙跳跃状态
// 核心逻辑：从滑墙状态触发，向墙的反方向施加跳跃力，上升结束后切换下落状态
// ========================================================

using UnityEngine;

public class Player_WallJumpState : EntityState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter(); // 激活跳跃动画、重置触发标记
        // 施加墙跳力：水平方向=墙跳力×反面向（远离墙壁），竖直方向=墙跳力Y（向上）
        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update(); // 保留基类：计时器、Y轴速度传递、冲刺检测等逻辑

        // 跳跃顶点检测：竖直速度≤0（上升结束开始下坠）→切换到下落状态
        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);

        // 二次贴墙检测：跳跃过程中再次碰到墙壁→切换回滑墙状态（支持连续墙跳）
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}