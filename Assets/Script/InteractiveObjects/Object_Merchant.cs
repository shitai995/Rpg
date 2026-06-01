// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:32
// 版本：V1.1
// 描述：商人NPC类（继承NPC基础功能，实现商店交互、刷新商品）
// ========================================================

using UnityEngine;

/// <summary>
/// 商人NPC：玩家可交互打开商店界面，支持刷新商品列表
/// </summary>
public class Object_Merchant : Object_NPC, IInteractable
{
    private Inventory_Player inventory;  // 玩家背包
    private Inventory_Merchant merchant; // 商人背包

    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<Inventory_Merchant>();
    }

    protected override void Update()
    {
        base.Update();

        // 测试：按 Z 键刷新商店物品
        if (Input.GetKeyDown(KeyCode.Z))
            merchant.FillShopList();
    }

    /// <summary>
    /// 玩家交互：打开商店界面
    /// </summary>
    public void Interact()
    {
        ui.merchantUI.SetupMerchantUI(merchant, inventory);
        ui.OpenMerchantUI(true);
    }

    /// <summary>
    /// 玩家进入触发范围：绑定玩家背包
    /// </summary>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        merchant.SetInventory(inventory);
    }

    /// <summary>
    /// 玩家离开触发范围：关闭商店、隐藏提示
    /// </summary>
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllTooltips();
        ui.OpenMerchantUI(false);
    }
}