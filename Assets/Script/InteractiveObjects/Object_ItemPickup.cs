// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-12 12:17:46
// 版本：V1.1
// 描述：场景可拾取道具物体（玩家触碰拾取到背包）
// ========================================================

using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    [Tooltip("拾取后获得的道具数据")]
    [SerializeField] private ItemDataSO itemData;

    private SpriteRenderer sr;
    private Inventory_Item itemToAdd;   // 待添加的背包物品实例
    private Inventory_Base inventory;   // 玩家背包组件

    private void Awake()
    {
        // 根据道具数据创建可存入背包的物品实例
        itemToAdd = new Inventory_Item(itemData);
    }

    private void OnValidate()
    {
        // 编辑器模式下自动更新图标与物体名称
        if (itemData == null) return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 获取触碰目标的背包组件
        inventory = collision.GetComponent<Inventory_Base>();
        if (inventory == null) return;

        // 检查背包是否可添加（有空位 or 可堆叠）
        bool canAddItem = inventory.CanAddItem() || inventory.StackableItem(itemToAdd) != null;

        if (canAddItem)
        {
            inventory.AddItem(itemToAdd);  // 添加到背包
            Destroy(gameObject);           // 销毁拾取物
        }
    }
}