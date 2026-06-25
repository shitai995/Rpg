// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 12:17:46
// 版本：V1.1
// 描述：场景可拾取道具物体（玩家触碰拾取到背包）
// ========================================================

using UnityEngine;

/// <summary>
/// 场景掉落物品：生成、物理弹跳、玩家触碰自动拾取
/// </summary>
public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] private Vector2 dropForce = new Vector2(3, 10); // 掉落弹射力度
    [SerializeField] private ItemDataSO itemData; // 物品数据

    [Space]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    // 编辑器验证：自动设置图标
    private void OnValidate()
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        SetupVisuals();
    }

    /// <summary>
    /// 外部调用：设置掉落物品并施加物理反弹
    /// </summary>
    public void SetupItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        // 随机左右方向弹出
        float xDropForce = Random.Range(-dropForce.x, dropForce.x);
        rb.linearVelocity = new Vector2(xDropForce, dropForce.y);
        col.isTrigger = false;
    }

    // 设置物品图标与名称
    private void SetupVisuals()
    {
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }

    // 落地后变成触发器、停止运动
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && !col.isTrigger)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    // 玩家触碰 → 拾取物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventory = collision.GetComponent<Inventory_Player>();
        if (inventory == null) return;

        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        Inventory_Storage storage = inventory.storage;

        // 材料 → 存入材料库
        if (itemData.itemType == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }

        // 普通物品 → 存入背包
        if (inventory.CanAddItem(itemToAdd))
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}