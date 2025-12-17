// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:32:37
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy,StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;

    }

    public override void Update()
    {
        base.Update();

        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
    }
}
