// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 22:14:54
// 版本：V1.1
// 描述：碎片技能核心类
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;// 当前生成的碎片实例
    private Entity_Health playerHealth;// 玩家血量组件

    [SerializeField] private GameObject shardPrefab;// 碎片预制体
    [SerializeField] private float detonateTime = 2;// 普通碎片的自动爆炸时间

    [Header("移动碎片升级配置")]
    [SerializeField] private float shardSpeed = 7;// 碎片向敌人移动的速度


    [Header("充能碎片升级配置")]
    [SerializeField] private int maxCharges = 3;// 碎片最大充能次数
    [SerializeField] private int currentChanges;// 当前剩余充能次数
    [SerializeField] private bool isRecharging;// 是否正在充能中

    [Header("传送碎片升级配置")]
    [SerializeField] private float shardExistDuration = 10;// 传送碎片的存在时长

    [Header("血量回溯碎片升级配置")]
    [SerializeField] private float saveHealthPercent;// 生成碎片时记录的玩家血量百分比


    protected override void Awake()
    {
        base.Awake();

        currentChanges = maxCharges;
        playerHealth = GetComponentInParent<Entity_Health>();
    }
    /// <summary>
    /// 尝试使用碎片技能
    /// </summary>
    public override void TryUseSkill()
    {
        if (CanUseSkill() == false)
            return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_Multicast))
            HandleShardMulticast();

        if(Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();

        if(Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealthRewind();
    }
    /// <summary>
    /// 处理血量回溯碎片逻辑
    /// 第一次使用：生成碎片 + 记录当前血量百分比
    /// 第二次使用：与碎片换位 + 恢复血量 + 触发冷却
    /// </summary>
    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            // 无碎片时：生成碎片，记录当前血量百分比
            CreateShard();
            saveHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            // 有碎片时：换位 + 恢复血量 + 触发技能冷却
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(saveHealthPercent);
            SetSkillOnCooldown();
        }
    }
    /// <summary>
    /// 处理传送碎片逻辑
    /// 第一次使用：生成碎片
    /// 第二次使用：与碎片换位 + 触发冷却
    /// </summary>
    private void HandleShardTeleport()
    {
        if(currentShard == null)
            CreateShard();
        else
        {
            // 有碎片时：换位 + 触发技能冷却
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }

    }
    /// <summary>
    /// 玩家与碎片换位核心逻辑
    /// 1. 交换玩家和碎片的位置
    /// 2. 触发碎片爆炸
    /// 3. 传送玩家到碎片原位置
    /// </summary>
    private void SwapPlayerAndShard()
    {
        // 记录双方当前位置
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;
        // 碎片移动到玩家原位置并爆炸
        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        // 调用玩家传送方法，移动到碎片原位置
        player.TeleportPlayer(shardPosition);
    }
    /// <summary>
    /// 处理充能型多段碎片逻辑
    /// 有剩余充能时生成追踪碎片，用完后自动开启充能协程
    /// </summary>
    private void HandleShardMulticast()
    {
        if (currentChanges <= 0)
            return;
        // 生成碎片并让其追踪最近敌人
        CreateShard();
        currentShard.MoveTowardsClosesTarget(shardSpeed);
        currentChanges--;
        // 未在充能时，开启充能协程
        if (isRecharging == false)
            StartCoroutine(ShardRechargeCo());
    }
    /// <summary>
    /// 碎片充能协程
    /// </summary>
    private IEnumerator ShardRechargeCo()
    {
        // 标记为充能中，防止重复开启协程
        isRecharging = true;
        // 循环充能：直到当前充能次数等于最大值
        while (currentChanges < maxCharges)
        {
            yield return new WaitForSeconds(cooldown);
            currentChanges++;
        }

        isRecharging = false;
    }
    /// <summary>
    /// 处理追踪敌人的移动碎片逻辑
    /// </summary>
    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosesTarget(shardSpeed);

        SetSkillOnCooldown();// 触发技能冷却
    }
    /// <summary>
    /// 处理基础碎片逻辑
    /// 生成碎片并触发技能冷却
    /// </summary>
    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }
    /// <summary>
    /// 生成碎片核心方法（主逻辑）
    /// 根据升级类型设置碎片爆炸时间，绑定传送碎片的爆炸回调
    /// </summary>
    public void CreateShard()
    {
        // 获取碎片爆炸时间
        float detonateTime = GetDetonateTime();

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        // 传送/回血回溯碎片：绑定爆炸回调（强制触发冷却）
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }
    /// <summary>
    /// 生成原始碎片
    /// </summary>
    public void CreateRawShard()
    {
        // 判断是否解锁移动/充能碎片升级（决定碎片是否追踪敌人）
        bool canMove = Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_Multicast);
       
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this,detonateTime,canMove,shardSpeed);
    }
    /// <summary>
    /// 获取碎片爆炸时间
    /// 传送类碎片返回长时长，普通碎片返回默认爆炸时间
    /// </summary>
    public float GetDetonateTime()
    {
        if(Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDuration;

        return detonateTime;
    }
    /// <summary>
    /// 强制触发技能冷却（碎片爆炸回调）
    /// 防止重复绑定回调，冷却完成后解除绑定
    /// </summary>
    private void ForceCooldown()
    {
        if(OnCooldown() == false)
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }

}
