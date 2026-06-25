// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 23:46:43
// 版本：V1.1
// 描述：玩家空中基础状态（继承自状态基类）
// 作用：统一处理空中通用逻辑（空中移动、空中攻击），供跳跃/下落等空中状态继承
// ========================================================

using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    /// <summary>
    /// 处理空中移动和空中攻击输入
    /// </summary>
    public override void Update()
    {
        // 调用基类Update：保留计时器、Y轴速度传递、冲刺检测等通用逻辑
        base.Update();

        // 空中移动：有水平输入时平滑过渡（无输入时保持惯性，不瞬间归零）
        if (player.moveInput.x != 0)
        {
            float targetX = player.moveInput.x * player.moveSpeed * player.inAirMoveMultiplier;
            float smoothedX = Mathf.Lerp(rb.linearVelocity.x, targetX, 10f * Time.deltaTime);
            player.SetVelocity(smoothedX, rb.linearVelocity.y);
        }

        // 空中攻击：检测攻击输入，切换到跳跃攻击状态
        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);
    }
}