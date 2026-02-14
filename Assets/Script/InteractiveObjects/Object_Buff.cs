// ========================================================
// 作者：娇娇 
// 创建时间：2026-02-05 15:37:00
// 版本：V1.1
// 描述：Buff道具核心类
// ========================================================

using System.Collections;
using UnityEngine;

/// <summary>
/// Buff数据结构（单条Buff的属性类型+数值）
/// 支持为单个Buff道具配置多个属性加成/减益
/// </summary>
[System.Serializable]
public class Buff
{
    public StatType type;
    public float value;
}
public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity_Stats statsToModify;


    [Header("Buff details")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4;//Buff持续时长
    [SerializeField] private bool canBeUsed = true;//是否可被使用（防止重复触发）


    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 1f;//漂浮动画速度（数值越大，上下浮动越快）
    [SerializeField] private float floatRange = .1f;//漂浮幅度（数值越大，上下浮动距离越远）
    private Vector3 startPosition;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;//记录初始位置
    }

    private void Update()
    {
        // 计算Y轴偏移（正弦函数实现平滑往复运动）
        float yOffset = Mathf.Sin(floatSpeed * Time.time) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed)
            return;

        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }

    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;
        ApplyBuff(true);

        yield return new WaitForSeconds(duration);

        ApplyBuff(false);
        Destroy(gameObject);
    }

    private void ApplyBuff(bool apply)
    {
        foreach (var buff in buffs)
        {
            if(apply)
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}

