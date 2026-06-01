// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:27:27
// 版本：V1.1
// 描述：传送卷轴物品效果配置
// ========================================================

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 传送卷轴效果逻辑
/// </summary>
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Portal Scroll", fileName = "Item effect data - PortalScroll")]
public class ItemEffect_PortalScroll : ItemEffect_DataSO
{
    /// <summary>
    /// 执行传送效果
    /// </summary>
    public override void ExecuteEffect()
    {
        // 城镇场景禁止使用传送
        if (SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("Cannot open portal in town!");
            return;
        }

        // 计算传送门生成位置
        // Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(player.facingDir * 1.5f, 0);

        // 激活传送门
        // Object_Portal.instnace.ActivatePortal(portalPosition, player.facingDir);
    }
}