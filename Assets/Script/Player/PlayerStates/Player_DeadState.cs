// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 17:18:25
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input.Disable();
        rb.simulated = false;
    }


}
