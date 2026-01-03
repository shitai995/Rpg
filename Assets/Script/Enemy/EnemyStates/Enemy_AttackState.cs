// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-18 23:07:15
// 版本：V1.1
// 描述：敌人攻击状态类
// ========================================================

using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
