// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:29:15
// 版本：V1.1
// 描述：场景传送路点，用于场景切换与重生定位
// ========================================================

using UnityEngine;

/// <summary>
/// 场景传送路点
/// </summary>
public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;     // 目标场景名
    [Space]
    [SerializeField] private RespawnType waypointType;   // 当前路点类型
    [SerializeField] private RespawnType conntedWaypoint;// 对应关联路点类型
    [SerializeField] private Transform respwanPoint;     // 重生位置节点
    [SerializeField] private bool canBeTriggered = true; // 是否可触发传送

    /// <summary>
    /// 获取当前路点类型
    /// </summary>
    public RespawnType GetWaypointType() => waypointType;

    /// <summary>
    /// 获取位置并关闭触发状态
    /// </summary>
    public Vector3 GetPositionAndSetTriggerFalse()
    {
        canBeTriggered = false;
        return respwanPoint == null ? transform.position : respwanPoint.position;
    }

    /// <summary>
    /// 编辑器校验：自动命名、绑定关联路点类型
    /// </summary>
    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + waypointType.ToString() + " - " + transferToScene;

        // 进出类型互相关联
        if (waypointType == RespawnType.Enter)
            conntedWaypoint = RespawnType.Exit;
        if (waypointType == RespawnType.Exit)
            conntedWaypoint = RespawnType.Enter;
    }

    /// <summary>
    /// 进入触发区域，执行场景切换
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeTriggered)
            return;

        GameManager.instance.ChangeScene(transferToScene, conntedWaypoint);
    }

    /// <summary>
    /// 离开触发区域，恢复触发状态
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}