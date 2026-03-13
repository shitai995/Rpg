// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 21:55:08
// 版本：V1.1
// 描述：碎片技能物体子类
// ========================================================

using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;// 碎片爆炸回调事件
    private Skill_Shard shardManager;

    [SerializeField] private GameObject vfxPrefab;// 爆炸特效预制体

    private Transform target;// 追踪的目标
    private float speed;// 碎片移动速度

    private void Update()
    {
        if (target == null)
            return;
        // 匀速向目标移动
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime) ;
    }
    /// <summary>
    /// 设置碎片追踪最近目标并指定移动速度
    /// 调用基类FindClosestTarget()查找最近敌人
    /// </summary>
    public void MoveTowardsClosesTarget(float speed) {
        target = FindClosestTarget();
        this.speed = speed;
    }

    /// <summary>
    /// 初始化碎片（基础版）
    /// 接收管理器引用，初始化伤害数据，设置定时爆炸
    /// </summary>
    public void SetupShard(Skill_Shard shardManager)
    {
        this.shardManager = shardManager;
        // 初始化基类的伤害相关数据
        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;
        // 获取碎片爆炸时间
        float detonationTime = shardManager.GetDetonateTime();
        // 定时触发爆炸
        Invoke(nameof(Explode),detonationTime);
    }

    /// <summary>
    /// 初始化碎片（重载版）
    /// 支持自定义爆炸时间、是否移动、移动速度
    /// </summary>
    public void SetupShard(Skill_Shard shardManager,float detonationTime,bool canMove,float shardSpeed)
    {
        this.shardManager = shardManager;
        playerStats = shardManager.player.stats;
        damageScaleData = shardManager.damageScaleData;

        Invoke(nameof(Explode), detonationTime);
        // 开启追踪移动
        if (canMove)
            MoveTowardsClosesTarget(shardSpeed);
    }
    /// <summary>
    /// 碎片爆炸核心方法
    /// 逻辑：造成范围伤害 → 生成爆炸特效 → 触发回调 → 销毁自身
    /// </summary>
    public void Explode()
    {
        // 1. 调用基类方法，对检测范围内的敌人造成伤害
        DamageEnemiesInRadius(transform,checkRadius);
        GameObject vfx = Instantiate(vfxPrefab, transform.position, Quaternion.identity);
        //vfx.GetComponentInChildren<SpriteRenderer>().color = shardManager.player.vfx.GetElementColor(usedElement);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }
    /// <summary>
    /// 2D碰撞触发：接触敌人时立即爆炸
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;
        // 立即爆炸
        Explode();
    }

}
