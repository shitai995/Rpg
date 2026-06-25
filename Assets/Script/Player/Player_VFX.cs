// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 17:28:32
// 版本：V1.1
// 描述：玩家专属特效管理类，继承实体特效基类，新增残影特效
// ========================================================

using UnityEngine;
using System.Collections;

public class Player_VFX : Entity_VFX
{
    /// <summary>
    /// 通用特效生成
    /// </summary>
    public void CreateEffectOf(GameObject effect, Transform target)
    {
        Instantiate(effect, target.position, Quaternion.identity);
    }
}