// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:32
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public class Object_Merchant : Object_NPC, IInteractable
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<Inventory_Merchant>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z))
            merchant.FillShopList();
    }

    public void Interact()
    {
        ui.merchantUI.SetupMerchantUI(merchant,inventory);
        ui.merchantUI.gameObject.SetActive(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        merchant.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.SwitchOffAllTooltips();
        ui.merchantUI.gameObject.SetActive(false);
    }

}
