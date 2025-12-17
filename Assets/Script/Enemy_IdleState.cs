// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 23:27:27
// 版本：V1.1
// 描述：
// ========================================================

using Unity.VisualScripting;
using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}

