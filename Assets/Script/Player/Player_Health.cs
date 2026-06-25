// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:29:40
// 版本：V1.1
// 描述：玩家生命值管理
// ========================================================

using UnityEngine;

/// <summary>
/// 玩家血量与死亡逻辑
/// </summary>
public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    /// <summary>
    /// 玩家死亡处理
    /// </summary>
    protected override void Die()
    {
        base.Die();

        // 打开死亡界面、记录位置并重启场景
         player.ui.OpenDeathScreenUI();
         //GameManager.instance.SetLastPlayerPosition(transform.position);
         //GameManager.instance.RestartScene();
    }
}