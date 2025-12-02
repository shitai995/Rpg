// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:37:11
// 版本：V1.1
// 描述：玩家闲置状态（继承地面基础状态）
// 核心逻辑：进入时停止移动，检测移动输入切换到移动状态，贴墙时禁止移动
// ========================================================

using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter(); // 激活闲置动画、重置触发标记
        // 进入闲置时停止水平移动，保留竖直速度（防止落地时速度异常）
        player.SetVelocity(0, rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update(); // 继承父类：跳跃、攻击输入检测，离地切换下落状态

        // 贴墙禁止移动：当前面向方向有输入且检测到墙壁→不切换移动状态
        if (player.moveInput.x == player.facingDir && player.wallDetected)
            return;

        // 移动输入检测：有水平输入→切换到移动状态
        if (player.moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);
    }
}