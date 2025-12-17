// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-02 23:11:13
// 版本：V1.1
// 描述：玩家跳跃攻击状态
// 核心逻辑：空中触发攻击后应用固定攻击速度，落地后触发攻击动画，攻击结束返回闲置状态
// ========================================================

using UnityEngine;
public class Player_JumpAttackState : PlayerState
{
    private bool touchedGround; // 落地标记（确保落地动画只触发一次）

    public Player_JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter(); // 激活跳跃攻击动画、重置触发标记
        touchedGround = false; // 初始化落地标记为未落地

        // 应用跳跃攻击固定速度（水平方向跟随当前面向，竖直方向使用配置值）
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update(); // 保留基类：计时器、Y轴速度传递、冲刺检测等逻辑

        // 落地检测：首次落地时触发攻击动画，停止水平移动
        if (player.groundDetected && !touchedGround)
        {
            touchedGround = true; // 标记已落地（防止重复触发）
            anim.SetTrigger("jumpAttackTrigger"); // 触发跳跃攻击落地动画
            player.SetVelocity(0, rb.linearVelocity.y); // 停止水平移动，保留竖直速度（落地缓冲）
        }

        // 攻击结束检测：动画触发完成且已落地→返回闲置状态
        if (triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}