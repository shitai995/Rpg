// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 15:43:04
// 版本：V1.1
// 描述：精灵弓箭手箭矢逻辑，支持命中、钉身、玩家反击反弹
// ========================================================

using UnityEngine;

/// <summary>精灵弓箭手箭矢，实现可被反击接口</summary>
public class Enemy_ArcherElfArrow : MonoBehaviour, ICounterable
{
    [SerializeField] private LayerMask whatIsTarget; // 可命中目标层级
    private Collider2D col;
    private Rigidbody2D rb;
    private Entity_Combat combat;
    private Animator anim;

    // 箭矢允许被玩家反击反弹
    public bool CanBeCountered => true;

    /// <summary>初始化箭矢速度、伤害组件与朝向</summary>
    public void SetupArrow(float xVelocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        this.combat = combat;
        rb.linearVelocity = new Vector2(xVelocity, 0);

        // 向左飞行则翻转贴图
        if (rb.linearVelocity.x < 0)
            transform.Rotate(0, 180, 0);
    }

    // 碰撞触发命中逻辑
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测碰撞物体属于目标层级
        if (((1 << collision.gameObject.layer) & whatIsTarget) != 0)
        {
            combat.PreformAttackOnTarget(collision.transform);
            StuckIntoTarget(collision.transform);
        }
    }

    /// <summary>箭矢钉在目标身上，禁用物理与碰撞，延时销毁</summary>
    private void StuckIntoTarget(Transform target)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        anim.enabled = false;

        transform.parent = target;
        Destroy(gameObject, 3);
    }

    /// <summary>被玩家反击：反向飞行，新增敌人为可攻击目标</summary>
    public void HandleCounter()
    {
        // 速度反向、翻转朝向
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * -1, 0);
        transform.Rotate(0, 180, 0);

        // 把敌人层级加入可命中列表
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        whatIsTarget |= 1 << enemyLayer;
    }
}