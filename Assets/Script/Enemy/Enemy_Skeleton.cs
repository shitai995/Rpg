// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:31:45
// 版本：V1.1
// 描述：骷髅敌人子类 - 初始化骷髅专属的状态机和各状态实例
// ========================================================

using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned;}

    protected override void Awake()
    {
        base.Awake();
        // 初始化各状态实例，绑定当前骷髅对象、状态机、对应动画参数名
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
   
    public void HandleCounter()
    {
        if (!CanBeCountered)
            return;
        stateMachine.ChangeState(stunnedState);
    }
}
