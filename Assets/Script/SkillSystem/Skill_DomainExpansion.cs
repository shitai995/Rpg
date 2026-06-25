// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-19 17:02:24
// 版本：V1.1
// 描述：领域扩张技能逻辑，包含减速、碎片攻击、时间回响三种进阶形态
// ========================================================

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 领域扩张技能
/// </summary>
public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab; // 领域预制体

    [Header("减速形态配置")]
    [SerializeField] private float slowDownPercent = 0.8f;      // 减速比例
    [SerializeField] private float slowDownDomainDuration = 5; // 领域持续时长

    [Header("碎片连发形态配置")]
    [SerializeField] private int shardsToCast = 10;             // 碎片发射总数
    [SerializeField] private float shardCastDomainSlow = 1;    // 减速比例
    [SerializeField] private float shardCastDomainDuration = 8; // 领域持续时长
    private float spellCastTimer;   // 施法计时器
    private float spellsPerSecond;  // 每秒施法次数

    [Header("时间回响形态配置")]
    [SerializeField] private int echoToCast = 8;               // 回响生成总数
    [SerializeField] private float echoCastDomainSlow = 1;     // 减速比例
    [SerializeField] private float echoCastDomainDuration = 6;// 领域持续时长
    [SerializeField] private float healthToRestoreWithEcho = 0.05f; // 回响附带回血比例

    [Header("领域基础参数")]
    public float maxDomainSize = 10;  // 领域最大半径
    public float expandSpeed = 3;     // 领域扩张速度

    private List<Enemy> trappedTargets = new List<Enemy>(); // 领域内敌人列表
    private Transform currentTarget;                        // 当前施法目标

    /// <summary>
    /// 生成领域区域
    /// </summary>
    public void CreateDomain()
    {
        spellsPerSecond = GetSpellsToCast() / GetDomainDuration();
        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    /// <summary>
    /// 领域内持续施法逻辑
    /// </summary>
    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        // 计时结束则执行一次施法
        if (currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1 / spellsPerSecond;
            currentTarget = null;
        }
    }

    /// <summary>
    /// 执行单次施法
    /// </summary>
    private void CastSpell(Transform target)
    {
        // 时间回响形态
        if (upgradeType == SkillUpgradeType.Domain_EchoSpam)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }
        // 碎片攻击形态
        else if (upgradeType == SkillUpgradeType.Domain_ShardSpam)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }

    /// <summary>
    /// 从领域内随机查找有效敌人
    /// </summary>
    private Transform FindTargetInDomain()
    {
        // 移除已死亡/空对象
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);
        if (trappedTargets.Count == 0) return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }

    /// <summary>
    /// 获取当前形态的领域持续时间
    /// </summary>
    public float GetDomainDuration()
    {
        return upgradeType switch
        {
            SkillUpgradeType.Domain_SlowingDown => slowDownDomainDuration,
            SkillUpgradeType.Domain_ShardSpam => shardCastDomainDuration,
            SkillUpgradeType.Domain_EchoSpam => echoCastDomainDuration,
            _ => 0
        };
    }

    /// <summary>
    /// 获取当前形态的减速比例
    /// </summary>
    public float GetSlowPercentage()
    {
        return upgradeType switch
        {
            SkillUpgradeType.Domain_SlowingDown => slowDownPercent,
            SkillUpgradeType.Domain_ShardSpam => shardCastDomainSlow,
            SkillUpgradeType.Domain_EchoSpam => echoCastDomainSlow,
            _ => 0
        };
    }

    /// <summary>
    /// 获取施法总数量
    /// </summary>
    private int GetSpellsToCast()
    {
        return upgradeType switch
        {
            SkillUpgradeType.Domain_ShardSpam => shardsToCast,
            SkillUpgradeType.Domain_EchoSpam => echoToCast,
            _ => 0
        };
    }

    /// <summary>
    /// 判断领域是否为即时形态
    /// </summary>
    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Domain_EchoSpam
            && upgradeType != SkillUpgradeType.Domain_ShardSpam;
    }

    /// <summary>
    /// 向领域内添加敌人
    /// </summary>
    public void AddTarget(Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }

    /// <summary>
    /// 清空领域内所有目标并解除减速
    /// </summary>
    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
            enemy.StopSlowDown();
        trappedTargets.Clear();
    }
}