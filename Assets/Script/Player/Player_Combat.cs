// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-28 00:10:24
// 版本：V1.1
// 描述：玩家战斗核心类
// ========================================================

using Unity.VisualScripting;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter attack details")]
    [SerializeField] private float counterRecovery = .1f;// 反击后恢复时长
    [SerializeField] private LayerMask whatIsCounterable;
    /// <summary>
    /// 执行反击攻击检测与处理
    /// 核心逻辑：遍历检测范围内的所有目标，触发可被反击目标的反击逻辑
    /// </summary>
    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;

        foreach (var target in GetDetectedColliders(whatIsCounterable))
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if(counterable == null)
                continue;

            if(counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasPerformedCounter = true;
            } 
            
        }
        return hasPerformedCounter;
    }

    public float GetCounterRecoveryDuration() => counterRecovery;
}
