// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 14:29:34
// 版本：V1.1
// 描述：所有道具效果的抽象基类，定义通用接口与行为
// ========================================================

using UnityEngine;

/// <summary>
/// 道具效果基类（ScriptableObject）
/// 所有具体道具效果（Buff/治疗/伤害等）均继承此类
/// </summary>
public class ItemEffect_DataSO : ScriptableObject
{
    [Header("效果配置")]
    [TextArea]
    [Tooltip("效果描述文本，用于UI显示")]
    public string effectDescription;

    // 玩家引用（受效果影响的目标）
    protected Player player;

    /// <summary>
    /// 检查效果是否可执行
    /// </summary>
    public virtual bool CanBeUsed()
    {
        return true;
    }

    /// <summary>
    /// 执行具体效果逻辑
    /// </summary>
    public virtual void ExecuteEffect()
    {

    }

    /// <summary>
    /// 绑定玩家对象，设置效果目标
    /// </summary>
    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// 解绑玩家（清理引用）
    /// </summary>
    public virtual void Unsubscribe()
    {

    }
}