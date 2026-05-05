// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 22:20:02
// 版本：V1.1
// 描述：玩家地面基础状态（继承状态基类）
// 作用：统一处理地面状态通用逻辑（跳跃、攻击输入，离地切换下落状态），供闲置/移动等地面状态继承
// ========================================================

using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update(); // 调用基类：计时器、Y轴速度传递、冲刺检测等通用逻辑

        // 离地检测：竖直速度向下且未检测到地面→切换到下落状态（防止地面状态浮空）
        if (rb.linearVelocity.y < 0 && !player.groundDetected)
            stateMachine.ChangeState(player.fallState);

        // 跳跃输入：检测跳跃键按下→切换到跳跃状态
        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        // 攻击输入：检测攻击键按下→切换到基础攻击状态
        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
        // 格挡输入
        if (input.Player.CounterAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.counterAttackState);

        if (input.Player.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
            stateMachine.ChangeState(player.swordThrowState);
    }
}