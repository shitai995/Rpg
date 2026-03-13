// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-24 00:59:57
// 版本：V1.1
// 描述：实体视觉特效（VFX）管理类
// ========================================================

using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;// 实体的精灵渲染器（用于切换材质实现视觉特效）
    private Entity entity;

    [Header("受击材质特效参数")]
    [SerializeField] private Material onDamageMaterial;// 受击时切换的材质（如红色高亮材质）
    [SerializeField] private float onDamageVfxDuration = .2f;// 受击特效持续时间
    private Material originalMaterial;// 缓存的原始材质（用于特效结束后恢复）
    private Coroutine onDamageVfxCoroutine;// 受击特效协程引用（用于中断重复触发的特效）

    [Header("攻击颜色")]
    [SerializeField] private Color hitVfxColor = Color.white;// 攻击命中特效默认颜色
    [SerializeField] private GameObject hitVfx; // 普通命中特效预制体
    [SerializeField] private GameObject critHitVfx;// 暴击命中特效预制体

    [Header("特效颜色")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    [SerializeField] private Color shockVfx = Color.yellow;
    private Color originalHitVfxColor;// 缓存命中特效原始颜色（用于元素特效结束后恢复）
    private Coroutine statusVfxCo;


    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        // 缓存原始材质（避免重复获取，同时防止材质实例化后丢失原引用）
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }
    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(duration, chillVfx));

        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(duration, burnVfx));

        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(duration, shockVfx));
    }
    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }
    /// <summary>
    /// 元素状态特效协程（内部执行逻辑）
    /// </summary>
    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = .25f;// 颜色闪烁间隔（秒）
        float timeHasPassed = 0;// 已流逝时间（用于判断是否达到持续时长）

        // 计算闪烁的高亮/暗化颜色（基于传入的元素颜色
        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        bool toggle = false;// 颜色切换开关
        // 循环执行颜色闪烁，直到达到持续时长
        while (timeHasPassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timeHasPassed = timeHasPassed + tickInterval;// 累加已流逝时间

        }

        sr.color = Color.white;
    }

    /// <summary>
    /// 创建攻击特效
    /// </summary>
    public void CreateOnHitVFX(Transform target, bool isCrit,ElementType element)
    {
        GameObject hitPerfab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPerfab, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);

        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);

    }
    /// <summary>
    /// 更新命中特效颜色
    /// </summary>
    public Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                return chillVfx;
            case ElementType.Fire:
                return burnVfx;
            case ElementType.Lightning:
                return shockVfx;

            default: return Color.white;
        }

    }
    /// <summary>
    /// 播放受击材质特效（对外公开的调用接口）
    /// 特性：重复调用时会中断上一次特效，重新开始计时
    /// </summary>
    public void PlayOnDamageVfx()
    {
        // 若上一次受击特效还在播放，先中断（防止多个协程同时修改材质）
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);
        // 启动新的受击特效协程，并保存引用
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    /// <summary>
    /// 受击材质特效协程（内部执行逻辑）
    /// 流程：切换受击材质 → 等待指定时长 → 恢复原始材质
    /// </summary>
    private IEnumerator OnDamageVfxCo()
    {
        // 切换为受击材质
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        // 恢复原始材质
        sr.material = originalMaterial;
    }
}
