// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-19 20:06:16
// 版本：V1.1
// 描述：领域展开技能实体（缩放、减速、生命周期管理）
// ========================================================

using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;

    private float expandSpeed = 2;
    private float duration;
    private float slowDownPercent = .9f;

    private Vector3 targetScale;
    private bool isShrinking;

    // 初始化领域参数
    public void SetupDomain(Skill_DomainExpansion domainManager)
    {
        this.domainManager = domainManager;

        duration = domainManager.GetDomainDuration();
        slowDownPercent = domainManager.GetSlowPercentage();
        expandSpeed = domainManager.expandSpeed;
        float maxSize = domainManager.maxDomainSize;

        targetScale = Vector3.one * maxSize;
        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    // 处理领域缩放（扩大/缩小）
    private void HandleScaling()
    {
        float sizeDiff = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDiff > .1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShrinking && sizeDiff < .1f)
            TerminateDomain();
    }

    // 销毁领域
    private void TerminateDomain()
    {
        domainManager.ClearTargets();
        Destroy(gameObject);
    }

    // 开始缩小领域
    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    // 敌人进入领域 → 减速
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;

        domainManager.AddTarget(enemy);
        enemy.SlowDownEntity(duration, slowDownPercent, true);
    }

    // 敌人离开领域 → 停止减速
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;

        enemy.StopSlowDown();
    }
}