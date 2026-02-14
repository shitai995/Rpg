// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 16:17:40
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;
    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        anim.enabled = false;
        col .enabled = false;


        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        stateMachine.SwitchOffStateMachine();
    }
}
