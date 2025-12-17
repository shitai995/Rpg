// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 23:39:06
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.groundDetected == false || enemy.wallDetected)   
            enemy.Flip();
    }
    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if(enemy.groundDetected == false || enemy.wallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }
}
