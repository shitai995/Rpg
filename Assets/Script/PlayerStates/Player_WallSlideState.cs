// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-30 16:38:04
// 版本：V1.1
// 描述：玩家爬墙滑降状态
// 核心逻辑：贴墙时缓速下滑，支持墙跳触发、离地切换下落、落地切换闲置并转向
// ========================================================

using UnityEngine;

public class Player_WallSlideState : EntityState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update(); // 保留基类：计时器、Y轴速度传递、冲刺检测等逻辑

        HandleWallSlide(); // 处理爬墙缓速下滑逻辑

        // 墙跳输入：按下跳跃键→切换到爬墙跳跃状态
        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.wallJumpState);

        // 离墙检测：未检测到墙壁→切换到下落状态
        if (!player.wallDetected)
            stateMachine.ChangeState(player.fallState);

        // 落地检测：检测到地面→切换到闲置状态，并翻转角色面向（远离墙壁）
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);

            if(player.facingDir != player.moveInput.x)
                player.Flip();
        }
    }

    /// <summary>
    /// 爬墙缓速逻辑：根据输入控制下滑速度
    /// </summary>
    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
        {
            // 按下向下输入：保持原有竖直速度（快速下滑），保留水平输入响应
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        }
        else
        {
            // 无向下输入：应用滑墙减速倍率（缓速下滑），保留水平输入响应
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSlideSlowMultiplier);
        }
    }
}