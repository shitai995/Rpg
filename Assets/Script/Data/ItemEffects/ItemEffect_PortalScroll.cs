// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:27:27
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Portal Scroll", fileName = "Item effect data - PortalScroll")]

public class ItemEffect_PortalScroll : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("Cannot open portal in town!");
            return;
        }

       // Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(player.facingDir * 1.5f, 0);

      //  Object_Portal.instnace.ActivatePortal(portalPosition, player.facingDir);
    }
}
