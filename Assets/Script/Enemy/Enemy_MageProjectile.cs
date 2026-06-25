// ========================================================
// 作者：娇娇 
// 创建时间：2026-06-25 23:40:22
// 版本：V1.1
// 描述：法师弹道法术弹丸，带抛物线弹道计算、命中爆炸销毁逻辑
// ========================================================

using UnityEngine;

/// <summary>法师法术弹道投射物</summary>
public class Enemy_MageProjectile : MonoBehaviour
{
    private Entity_Combat combat;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    [SerializeField] private float arcHeight = 2f;       // 弹道最大拱高
    [SerializeField] private LayerMask whatCanCollideWith;// 可碰撞目标层级

    /// <summary>初始化弹丸，计算抛物线发射速度</summary>
    public void SetupProjectile(Transform target, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.enabled = false;
        this.combat = combat;

        // 计算抛物线初速度并赋值给刚体
        Vector2 velocity = CalculateBallisticVelocity(transform.position, target.position);
        rb.linearVelocity = velocity;
    }

    // 碰撞触发命中逻辑
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 校验碰撞对象层级
        if (((1 << collision.gameObject.layer) & whatCanCollideWith) != 0)
        {
            combat.PreformAttackOnTarget(collision.transform);

            // 停止物理运动，开启爆炸动画，延时销毁
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            anim.enabled = true;
            col.enabled = false;
            Destroy(gameObject, 2f);
        }
    }

    /// <summary>计算从起点到目标的抛物线初速度，保证弹道最高点不低于设定拱高</summary>
    private Vector2 CalculateBallisticVelocity(Vector2 start, Vector2 end)
    {
        // 结合全局重力与自身重力缩放，获取实际重力值
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        // 计算水平、竖直位移差
        float displacementY = end.y - start.y;
        float displacementX = end.x - start.x;

        // 取更高值，保证弹道顶点高于目标高度
        float peakHieght = Mathf.Max(arcHeight, end.y - start.y + 0.1f);

        // 上升到弹道顶点耗时
        float timeToApex = Mathf.Sqrt(2 * peakHieght / gravity);
        // 从顶点下落至目标耗时
        float timeFromApex = Mathf.Sqrt(2 * (peakHieght - displacementY) / gravity);
        // 全程飞行总时长
        float totalTime = timeToApex + timeFromApex;

        // 竖直向上初速度
        float velocityY = Mathf.Sqrt(2 * gravity * peakHieght);
        // 水平匀速速度
        float velocityX = displacementX / totalTime;

        return new Vector2(velocityX, velocityY);
    }
}