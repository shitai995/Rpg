
// ========================================================
// 作者：娇娇 
// 创建时间：2026-01-02 16:45:02
// 版本：V1.1
// 描述：游戏防御属性分组类，集中封装实体的物理防御与元素抗性属性
// ========================================================

using UnityEngine;
using System;

[Serializable]
public class Stat_DefenseGroup
{


    public Stat armor;// 物理护甲值
    public Stat evasion;// 闪避率（百分比）


    public Stat fireRes;// 火焰抗性
    public Stat iceRes;// 冰霜抗性
    public Stat lightningRes;// 雷电抗性
}
