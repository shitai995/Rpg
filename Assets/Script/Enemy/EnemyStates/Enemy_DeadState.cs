// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 16:17:40
// 版本：V1.1
// 描述：敌人死亡状态类
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
        anim.enabled = false;// 1. 关闭动画组件：死亡后无需播放任何动画
        col .enabled = false;// 2. 禁用碰撞体


        rb.gravityScale = 12;// 增大重力缩放，让敌人快速掉落
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);// 设置刚体线速度：保留水平速度，垂直方向给向上的初速度
        // 4. 关闭敌人状态机
        stateMachine.SwitchOffStateMachine();
    }
}
