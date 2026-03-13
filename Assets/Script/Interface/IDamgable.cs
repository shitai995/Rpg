// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-27 20:44:24
// 版本：V1.1
// 描述：可受击接口
// ========================================================

using UnityEngine;

public interface IDamgable
{
    public bool TakeDamage(float damage,float elementalDamage,ElementType element, Transform damageDealer);
}
