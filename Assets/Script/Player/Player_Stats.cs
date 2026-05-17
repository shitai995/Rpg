// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-16 13:18:15
// 版本：V1.1
// 描述：玩家属性管理类（扩展Buff系统）
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff;          // 当前生效的Buff唯一标识列表
    private Inventory_Player inventory;       // 玩家背包引用

    protected override void Awake()
    {
        base.Awake();
        activeBuff = new List<string>();
        inventory = GetComponent<Inventory_Player>();
    }

    /// <summary>
    /// 判断是否可施加指定Buff（防重复叠加）
    /// </summary>
    public bool CanApplyBuffOf(string source)
    {
        return !activeBuff.Contains(source);
    }

    /// <summary>
    /// 应用一组Buff效果（开启协程计时）
    /// </summary>
    public void ApplyBuff(BuffEffectData[] buffsToAply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffsToAply, duration, source));
    }

    /// <summary>
    /// Buff持续协程：添加修饰 → 等待时长 → 移除修饰
    /// </summary>
    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBuff.Add(source); // 标记Buff为激活

        // 给对应属性添加加成
        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration); // 等待持续时间结束

        // 移除属性加成
        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        inventory.TriggerUpdateUI(); // 刷新UI显示
        activeBuff.Remove(source);   // 取消Buff标记
    }
}