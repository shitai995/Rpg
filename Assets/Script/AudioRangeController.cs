// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:26:28
// 版本：V1.1
// 描述：音频距离衰减控制器，根据与玩家距离动态调节音量，附带场景可视化范围
// ========================================================

using UnityEngine;

/// <summary>
/// 空间音频距离衰减控制器
/// </summary>
public class AudioRangeController : MonoBehaviour
{
    private AudioSource source;
    private Transform player;

    [SerializeField] private float minDistanceToHearSound = 12f; // 有效收听最大距离
    [SerializeField] private bool showGizmo;                    // 是否在场景视图显示范围线框
    private float maxVolume;                                     // 音频最大音量

    private void Start()
    {
        player = Player.instance.transform;
        source = GetComponent<AudioSource>();
        maxVolume = source.volume;
    }

    private void Update()
    {
        if (player == null) return;

        // 计算与玩家的距离
        float distance = Vector2.Distance(player.position, transform.position);
        float factor = Mathf.Clamp01(1f - distance / minDistanceToHearSound);
        // 指数衰减计算目标音量
        float targetVolume = Mathf.Lerp(0f, maxVolume, factor * factor);
        // 平滑过渡音量
        source.volume = Mathf.Lerp(source.volume, targetVolume, Time.deltaTime * 3f);
    }

    /// <summary>
    /// 场景视图绘制音频范围线框
    /// </summary>
    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, minDistanceToHearSound);
        }
    }
}