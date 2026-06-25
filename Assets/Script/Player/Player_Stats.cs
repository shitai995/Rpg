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
    private List<string> activeBuff = new List<string>();    // 正在生效的Buff来源标识
    private Inventory_Player inventory;                      // 玩家背包

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    // 判断是否可以施加该来源的Buff（防止重复叠加）
    public bool CanApplyBuffOf(string source)
    {
        return !activeBuff.Contains(source);
    }

    // 施加一组Buff
    public void ApplyBuff(BuffEffectData[] buffsToApply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffsToApply, duration, source));
    }

    // Buff持续协程：计时结束后自动移除属性加成
    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBuff.Add(source);

        // 添加所有属性加成
        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration);

        // 移除所有属性加成
        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        inventory.TriggerUpdateUI();
        activeBuff.Remove(source);
    }
}