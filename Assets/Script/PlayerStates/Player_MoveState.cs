// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:35:37
// 版本：V1.1
// 描述：玩家移动状态（继承地面基础状态）
// 核心逻辑：持续应用移动速度，无输入/贴墙时返回闲置状态，复用地面跳跃、攻击等通用行为
// ========================================================

using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Update()
    {
        base.Update(); // 继承父类：跳跃、攻击输入检测，离地切换下落状态

        // 退出移动状态条件：无水平输入 或 检测到贴墙→切换到闲置状态
        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);

        // 应用移动速度：水平方向=输入×移动速度，保留竖直速度（如落地缓冲）
        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}