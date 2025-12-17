// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-17 22:31:45
// 版本：V1.1
// 描述：
// ========================================================

using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
}
