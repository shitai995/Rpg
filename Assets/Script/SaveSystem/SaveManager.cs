// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-22 12:14:33
// 版本：V1.1
// 描述：全局存档管理器，统一处理游戏存档的加载、保存与删除
// ========================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 存档管理单例类
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "unityalexdev.json"; // 存档文件名
    [SerializeField] private bool encryptData = true;              // 是否加密存档

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        // 初始化文件读写器
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        // 注册场景加载事件，每次切场景重新查找存档对象
        SceneManager.sceneLoaded += OnSceneLoaded;
        // 查找场景中所有可存档对象
        allSaveables = FindISaveables();

        yield return null;
        LoadGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(OnSceneLoadedCo());
    }

    private IEnumerator OnSceneLoadedCo()
    {
        allSaveables = FindISaveables();
        // 等待两帧：第一帧确保 Awake 执行，第二帧确保 Start 执行
        yield return null;
        yield return null;
        LoadGame();
    }

    /// <summary>
    /// 加载游戏存档
    /// </summary>
    private void LoadGame()
    {
        gameData = dataHandler.LoadData();

        // 无存档则新建存档数据
        if (gameData == null)
        {
            Debug.Log("未找到存档，创建新存档");
            gameData = new GameData();
            return;
        }

        // 逐个执行数据读取
        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);
    }

    /// <summary>
    /// 保存游戏存档
    /// </summary>
    public void SaveGame()
    {
        // 收集所有可存档对象数据
        foreach (var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
    }

    /// <summary>
    /// 获取当前存档数据
    /// </summary>
    public GameData GetGameData() => gameData;

    /// <summary>
    /// 删除存档数据（编辑器右键菜单）
    /// </summary>
    [ContextMenu("*** Delete save data ***")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
        LoadGame();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 应用退出时自动存档
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    /// <summary>
    /// 查找场景内所有实现ISaveable接口的对象
    /// </summary>
    private List<ISaveable> FindISaveables()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
}