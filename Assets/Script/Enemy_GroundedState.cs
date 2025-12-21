// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-20 15:24:50
// 版本：V1.1
// 描述：检测玩家随后进入战斗状态
// ========================================================

using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if(enemy.PlayerDetected()==true)
            stateMachine.ChangeState(enemy.battleState);

    }
   
}
