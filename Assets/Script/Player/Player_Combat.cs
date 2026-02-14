// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-28 00:10:24
// 版本：V1.1
// 描述：
// ========================================================

using Unity.VisualScripting;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter attack details")]
    [SerializeField] private float counterRecovery = .1f;
    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;

        foreach (var target in GetDetectedColliders())
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
