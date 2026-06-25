// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-22 12:14:52
// 版本：V1.1
// 描述：文件数据读写处理器，实现游戏存档的保存、读取、删除与简易加解密
// ========================================================

using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 本地文件数据读写工具类
/// </summary>
public class FileDataHandler
{
    private string fullPath;       // 存档文件完整路径
    private bool encrpyData;       // 是否开启数据加密
    private readonly string codeWord = "unityalexdev.com"; // 加解密密钥

    /// <summary>
    /// 构造函数，初始化存档路径与加密配置
    /// </summary>
    public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
        this.encrpyData = encryptData;
    }

    /// <summary>
    /// 保存游戏数据到本地文件
    /// </summary>
    public void SaveData(GameData gameData)
    {
        try
        {
            // 不存在目录则创建
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            // 序列化为JSON
            string dataToSave = JsonUtility.ToJson(gameData, true);

            // 加密数据
            if (encrpyData)
                dataToSave = EncryptDecrypt(dataToSave);

            // 写入文件
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            using (StreamWriter write = new StreamWriter(stream))
            {
                write.Write(dataToSave);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"保存存档失败：{fullPath}\n{e}");
        }
    }

    /// <summary>
    /// 从本地文件读取游戏数据
    /// </summary>
    public GameData LoadData()
    {
        GameData loadData = null;
        // 检测文件是否存在
        if (!File.Exists(fullPath))
            return loadData;

        try
        {
            string dataToLoad;
            // 读取文件内容
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                dataToLoad = reader.ReadToEnd();
            }

            // 解密数据
            if (encrpyData)
                dataToLoad = EncryptDecrypt(dataToLoad);

            // 反序列化为数据对象
            loadData = JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
            Debug.LogError($"读取存档失败：{fullPath}\n{e}");
        }

        return loadData;
    }

    /// <summary>
    /// 删除当前存档文件
    /// </summary>
    public void Delete()
    {
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    /// <summary>
    /// 异或加解密（加密/解密逻辑一致）
    /// </summary>
    private string EncryptDecrypt(string data)
    {
        string modifedData = string.Empty;
        for (int i = 0; i < data.Length; i++)
        {
            modifedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifedData;
    }
}