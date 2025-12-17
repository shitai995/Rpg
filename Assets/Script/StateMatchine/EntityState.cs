// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-11 23:18:09
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine; // 状态机管理器（用于切换状态）
    protected string animBoolName; // 动画控制器对应的布尔参数名

    protected Animator anim; // 动画组件（从Player获取）
    protected Rigidbody2D rb; // 2D刚体组件（从Player获取）

    protected float stateTimer; // 状态计时器（控制状态持续时间）
    protected bool triggerCalled; // 动画触发标记（避免重复攻击等行为）

    public EntityState(StateMachine stateMachine,string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true); // 激活状态动画
        triggerCalled = false; // 重置触发标记
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // 计时器倒计时
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false); // 关闭状态动画
    }

    /// <summary>
    /// 动画事件回调：标记触发已执行（避免重复行为）
    /// </summary>
    public void CallAnimtionTrigger()
    {
        triggerCalled = true;
    }


}
