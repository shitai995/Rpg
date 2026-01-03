// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 16:06:48
// 版本：V1.1
// 描述：
// ========================================================

using System;
using UnityEngine;
[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
