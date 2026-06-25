// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-07 21:15:28
// 版本：V1.1
// 描述：技能树连接处理器
// ========================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Ui_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;// 子节点的连接处理器
    public NodeDirectionType direction;// 连接线方向
    [Range(100f, 350f)] public float length;// 连接线长度
    [Range(-50f, 50f)] public float rotation;// 连接线旋转偏移
}
[ExecuteAlways]
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private Ui_TreeConnectDetails[] connectionDetails;// 连接详情数组
    [SerializeField] private UI_TreeConnection[] connections;// 连接线组件数组

    private Image connectionImage;// 当前绑定的连接线Image组件
    private Color originalColor;// 连接线原始颜色

    private void Awake()
    {
        if (connectionImage != null)
            originalColor = connectionImage.color;
    }
    // 获取当前节点的所有子节点
    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> childrenToReturn = new List<UI_TreeNode>();

        foreach (var node in connectionDetails)
        {
            if (node.childNode != null)
                childrenToReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
        }

        return childrenToReturn.ToArray();
    }
    // 更新当前节点的所有连接线
    private void UpdateConnections()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];
            // 1. 获取连接线的连接点位置
            Vector2 targetPosition = connection.GetConnectionPoint(rect);
            Image connectionImage = connection.GetConnectionImage();
            // 设置连接线的方向、长度、旋转偏移
            connection.DirectConnection(detail.direction, detail.length, detail.rotation);
            // 4. 同步子节点位置到连接点，并传递连接线Image组件
            if (detail.childNode == null)
                continue;
            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }
    /// <summary>
    /// 递归更新所有节点的连接状态（当前节点+所有子节点）
    /// </summary>
    public void UpdateAllConnections()
    {
        // 1. 更新当前节点的连接线
        UpdateConnections();
        // 2. 递归更新所有子节点的连接线
        foreach (var node in connectionDetails)
        {
            if (node.childNode == null) continue;
            node.childNode?.UpdateConnections();
        }
    }
    /// <summary>
    /// 解锁/锁定连接线（修改颜色）
    /// </summary>
    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
    // 编辑器校验：保证配置与连接线数组长度一致，实时更新连接状态
    private void OnValidate()
    {
        if (connectionDetails.Length <= 0)
            return;

        if (connectionDetails.Length != connections.Length)
        {
            //Debug.Log("详细信息数量应与链接数量相同. - " + gameObject.name);
            return;
        }
        UpdateAllConnections();
    }
}

