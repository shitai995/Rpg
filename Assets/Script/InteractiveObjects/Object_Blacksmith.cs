// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:32
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

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

    public void Interact()
    {
        ui.storageUI.SetupStorageUI(storage);
        ui.craftUI.SetupCraftUI(storage);


        ui.storageUI.gameObject.SetActive(true);
        //ui.craftUI.gameObject.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.SwitchOffAllTooltips();
        ui.storageUI.gameObject.SetActive(false);
        ui.craftUI.gameObject.SetActive(false);
    }

}
