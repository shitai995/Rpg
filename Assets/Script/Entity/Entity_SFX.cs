// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:28:53
// 版本：V1.1
// 描述：实体音效管理
// ========================================================

using UnityEngine;

/// <summary>
/// 实体战斗音效组件
/// </summary>
public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("音效标识名")]
    [SerializeField] private string attackHit;   // 攻击命中音效
    [SerializeField] private string attackMiss;  // 攻击落空音效
    [Space]
    [SerializeField] private float soundDistance = 15f; // 音效播放范围
    [SerializeField] private bool showGizmo;           // 是否绘制范围辅助线

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    /// <summary>
    /// 播放攻击命中音效
    /// </summary>
    public void PlayAttackHit()
    {
        //AudioManager.instance.PlaySFX(attackHit, audioSource, soundDistance);
    }

    /// <summary>
    /// 播放攻击落空音效
    /// </summary>
    public void PlayAttackMiss()
    {
        //AudioManager.instance.PlaySFX(attackMiss, audioSource, soundDistance);
    }

    /// <summary>
    /// 绘制音效范围球
    /// </summary>
    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, soundDistance);
        }
    }
}