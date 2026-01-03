// ========================================================
// 作者：娇娇 
// 创建时间：2025-11-29 15:36:41
// 版本：V1.1
// 描述：状态机核心类，负责状态的初始化、切换与更新管理
// 核心作用：统一管控所有玩家状态的生命周期，解耦状态间的直接依赖
// ========================================================

using UnityEngine;

public class StateMachine
{

    public EntityState currentState { get; private set; }
    public bool canChangesState;

    /// <summary>
    /// 初始化状态机
    /// 设定初始状态并执行其进入逻辑（游戏启动时调用一次）
    /// </summary>
    /// <param name="startState">游戏初始状态（如玩家闲置状态）</param>
    public void Initialize(EntityState startState)
    {
        canChangesState = true;
        currentState = startState;
        currentState.Enter(); // 执行初始状态的进入逻辑（如激活动画、初始化参数）
    }
    public void ChangeState(EntityState newState)
    {
        if (canChangesState == false)
            return; 
        currentState.Exit(); // 执行旧状态的退出逻辑（如关闭动画、清理参数）
        currentState = newState; // 更新当前状态为新状态
        currentState.Enter(); // 执行新状态的进入逻辑
    }

    /// <summary>
    /// 驱动当前激活状态的帧更新
    /// 在Player的Update中调用，确保状态逻辑每帧执行
    /// </summary>
    public void UpdateActiveState()
    {
        currentState.Update(); // 调用当前状态的Update方法（处理输入、状态切换条件等）
    }

    
    public void SwitchOffStateMachine() => canChangesState = false;
}