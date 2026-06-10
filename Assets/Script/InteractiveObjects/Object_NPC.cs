// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-20 14:21:32
// 版本：V1.1
// 描述：NPC 基础交互类（面向玩家转向、交互提示浮动）
// ========================================================

using UnityEngine;

/// <summary>
/// NPC 基类：实现自动转向玩家、交互提示浮动效果
/// </summary>
public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;

    [SerializeField] private Transform npc;               // NPC 模型节点
    [SerializeField] private GameObject interactToolTip; // 交互提示（按E图标）
    private bool facingRight = true;

    [Header("浮动提示设置")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition; // 提示初始位置

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition = interactToolTip.transform.position;
        interactToolTip.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }

    // 交互提示上下浮动动画
    private void HandleToolTipFloat()
    {
        if (interactToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = startPosition + new Vector3(0, yOffset);
        }
    }

    // NPC 自动面向玩家
    private void HandleNpcFlip()
    {
        if (player == null || npc == null)
            return;

        // 玩家在左侧 → NPC 左转
        if (npc.position.x > player.position.x && facingRight)
        {
            npc.Rotate(0, 180, 0);
            facingRight = false;
        }
        // 玩家在右侧 → NPC 右转
        else if (npc.position.x < player.position.x && !facingRight)
        {
            npc.Rotate(0, 180, 0);
            facingRight = true;
        }
    }

    // 玩家进入范围 → 显示交互提示
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        player = collision.transform;
        interactToolTip.SetActive(true);
    }

    // 玩家离开范围 → 隐藏提示
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        interactToolTip.SetActive(false);
        player = null;
    }
}