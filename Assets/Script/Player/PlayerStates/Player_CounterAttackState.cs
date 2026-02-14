// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 23:49:16
// 版本：V1.1
// 描述：玩家反击状态类
// ========================================================

using UnityEngine;

public class Player_CounterAttackState : PlayerState
{

    private Player_Combat combat;// 玩家战斗组件
    private bool counteredSomebody;// 是否成功反击到目标
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = combat.GetCounterRecoveryDuration();
        // 2. 触发核心反击逻辑，获取是否成功反击到目标的结果
        counteredSomebody = combat.CounterAttackPerformed();

        anim.SetBool("counterAttackPerformed", counteredSomebody);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.linearVelocity.y);
        // 退出条件1：动画触发器被调用（如反击动画播放完成，触发了动画事件）
        // 优先级最高：确保动画播放完成后再切换状态，避免动画中断
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
        // 退出条件2：状态计时器耗尽，且未成功反击到任何目标
        // 用于处理“反击落空”的逻辑：达到最小持续时间后，返回闲置状态
        if (stateTimer < 0 && counteredSomebody == false)
            stateMachine.ChangeState(player.idleState);
    }
}
