// ========================================================
// 作者：娇娇 
// 创建时间：2026-02-05 15:37:00
// 版本：V1.1
// 描述：Buff道具核心类
// ========================================================

using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private Player_Stats statsToModify;


    [Header("Buff details")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4;//Buff持续时长


    [Header("Floaty movement")]
    [SerializeField] private float floatSpeed = 1f;//漂浮动画速度（数值越大，上下浮动越快）
    [SerializeField] private float floatRange = .1f;//漂浮幅度（数值越大，上下浮动距离越远）
    private Vector3 startPosition;

    private void Awake()
    {
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
        statsToModify = collision.GetComponent<Player_Stats>();
        if (statsToModify == null) return;
        // 检查是否可施加该Buff
        if (statsToModify.CanApplyBuffOf(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }
}

