using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
public class ScriptCreateInit : UnityEditor.AssetModificationProcessor
{
    /// <summary>
    /// 在资源创建时调用
    /// </summary>
    /// <param name="path">自动传入资源路径</param>
    public static void OnWillCreateAsset(string path)
    {
        // 只处理 .meta 文件，实际的 .cs 文件可能还没创建完成
        if (!path.EndsWith(".cs.meta")) return;

        // 获取实际的 .cs 文件路径
        string scriptPath = path.Replace(".meta", "");

        // 使用延迟调用确保文件已经创建完成
        EditorApplication.delayCall += () =>
        {
            ProcessScriptFile(scriptPath);
        };
    }
    private static void ProcessScriptFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"文件不存在，跳过处理: {path}");
            return;
        }
        try
        {
            string originalContent = File.ReadAllText(path);

            // 你的头部注释模板
            string header = "// ========================================================\r\n"
                          + "// 作者：娇娇 \r\n"
                          + "// 创建时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n"
                          + "// 版本：V1.1\r\n"
                          + "// 描述：\r\n"
                          + "// ========================================================\r\n\r\n";

            // 检查是否已经添加过头部（防止重复处理）
            if (!originalContent.Contains("// 作者：娇娇"))
            {
                string newContent = header + originalContent;
                File.WriteAllText(path, newContent);
                // 刷新资源数据库
                AssetDatabase.Refresh();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"处理脚本文件时出错 {path}: {e.Message}");
        }
    }
}