// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 22:46:16
// 版本：V1.1
// 描述：玩家下落状态（继承空中基础状态）
// 核心逻辑：复用空中移动/空中攻击通用逻辑，新增落地、撞墙后的状态切换
// ========================================================

using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update(); // 继承父类：空中移动、空中攻击输入检测

        // 记录跳跃键按下时间（用于 Jump Buffer）
        if (input.Player.Jump.WasPressedThisFrame())
            player.lastJumpPressTime = Time.time;

        // Coyote Time：离开平台的短暂时间内仍可跳跃
        if (Time.time - player.lastJumpPressTime <= player.jumpBufferTime
            && Time.time - player.lastGroundedTime <= player.coyoteTime)
        {
            player.lastJumpPressTime = 0; // 消耗缓冲
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        // 落地检测：有跳跃缓冲则直接起跳，否则回到闲置
        if (player.groundDetected)
        {
            if (Time.time - player.lastJumpPressTime <= player.jumpBufferTime)
            {
                player.lastJumpPressTime = 0; // 消耗缓冲
                stateMachine.ChangeState(player.jumpState);
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }

        // 撞墙检测：蹬墙跳冷却过后才检测墙壁，避免刚跳出去就被拉回
        if (Time.time - player.lastWallJumpTime > player.wallJumpWallDetectDelay && player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}