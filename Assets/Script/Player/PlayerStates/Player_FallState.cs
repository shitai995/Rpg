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

        // 落地检测：检测到地面→切换到闲置状态
        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);

        // 撞墙检测：空中检测到墙壁→切换到滑墙状态
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}