// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 22:45:40
// 版本：V1.1
// 描述：玩家跳跃状态（继承空中基础状态）
// 核心逻辑：进入时施加跳跃力，上升到顶点（竖直速度≤0）后切换下落状态，跳过空中攻击时不切换
// ========================================================

using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter(); // 激活跳跃动画、重置触发标记
        // 施加跳跃力：保留水平速度（实现斜跳），竖直方向设置跳跃力
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update(); // 继承父类：空中移动、空中攻击输入检测

        if (rb.linearVelocity.y > 0 && input.Player.Jump.WasReleasedThisFrame())
            player.SetVelocity(rb.linearVelocity.x, rb.linearVelocity.y * player.jumpCutMultiplier);

        // 跳跃顶点检测：竖直速度≤0（上升结束开始下坠），且未处于空中攻击状态→切换到下落状态
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}