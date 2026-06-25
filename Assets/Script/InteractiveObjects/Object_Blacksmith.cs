// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:32
// 版本：V1.1
// 描述：铁匠铺NPC（开放储物栏 + 合成系统）
// ========================================================

using UnityEngine;

/// <summary>
/// 铁匠NPC：玩家交互打开储物栏与合成界面
/// </summary>
public class Object_Blacksmith : Object_NPC, IInteractable
{
    private Animator anim;
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBlacksmith", true);
    }

    /// <summary>
    /// 玩家交互：打开储物 + 合成界面
    /// </summary>
    public override void Interact()
    {
        base.Interact();
        ui.storageUI.SetupStorageUI(storage);
        ui.craftUI.SetupCraftUI(storage);

        ui.OpenStorageUI(true);
    }

    /// <summary>
    /// 玩家进入范围：绑定玩家背包
    /// </summary>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    /// <summary>
    /// 玩家离开：关闭所有界面
    /// </summary>
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllTooltips();
        ui.OpenStorageUI(false);
    }
}