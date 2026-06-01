//// ========================================================
//// 作者：娇娇 
//// 创建时间：2026-05-29 22:26:27
//// 版本：V1.1
//// 描述：游戏全局管理器（单例），负责场景切换、玩家重生、存档读写、场景过渡动画
//// ========================================================

//using System.Collections;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.SceneManagement;

///// <summary>
///// 游戏核心管理器，统筹场景加载、角色重生、存档数据交互
///// </summary>
//public class GameManager : MonoBehaviour, ISaveable
//{
//    public static GameManager instance;

//    private Vector3 lastPlayerPosition;
//    private string lastScenePlayed;
//    private bool dataLoaded;

//    private void Awake()
//    {
//        // 单例初始化
//        if (instance != null && instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        instance = this;
//        DontDestroyOnLoad(gameObject);
//    }

//    /// <summary>
//    /// 继续游戏，加载上一次游玩场景
//    /// </summary>
//    public void ContinuePlay()
//    {
//        ChangeScene(lastScenePlayed, RespawnType.NonSpecific);
//    }

//    /// <summary>
//    /// 重新加载当前场景
//    /// </summary>
//    public void RestartScene()
//    {
//        string currentScene = SceneManager.GetActiveScene().name;
//        ChangeScene(currentScene, RespawnType.NonSpecific);
//    }

//    /// <summary>
//    /// 切换场景，执行存档保存与过渡动画
//    /// </summary>
//    public void ChangeScene(string sceneName, RespawnType respawnType)
//    {
//        SaveManager.instance.SaveGame();
//        Time.timeScale = 1f;
//        StartCoroutine(ChangeSceneCoroutine(sceneName, respawnType));
//    }

//    /// <summary>
//    /// 场景切换协程：淡出画面 → 加载场景 → 读取存档 → 淡入画面 → 重置玩家位置
//    /// </summary>
//    private IEnumerator ChangeSceneCoroutine(string sceneName, RespawnType respawnType)
//    {
//       // UI_FadeScreen fadeScreen = FindFadeScreenUI();
//        // 画面淡出（透明 → 黑屏）
//        fadeScreen.DoFadeOut();
//        yield return fadeScreen.fadeEffectCo;

//        // 加载目标场景
//        SceneManager.LoadScene(sceneName);
//        dataLoaded = false;

//        // 等待存档数据加载完成
//        yield return null;
//        while (!dataLoaded)
//        {
//            yield return null;
//        }

//        // 画面淡入（黑屏 → 透明）
//       // fadeScreen = FindFadeScreenUI();
//        fadeScreen.DoFadeIn();

//        Player player = Player.instance;
//        if (player == null) yield break;

//        // 设置玩家重生位置
//        Vector3 spawnPos = GetNewPlayerPosition(respawnType);
//        if (spawnPos != Vector3.zero)
//            player.TeleportPlayer(spawnPos);
//    }

//    /// <summary>
//    /// 查找画面过渡遮罩组件
//    /// </summary>
//   // private UI_FadeScreen FindFadeScreenUI()
//   // {
//       // if (UI.instance != null)
//       //     return UI.instance.fadeScreenUI;
//       // return FindFirstObjectByType<UI_FadeScreen>();
//  //  }

//    /// <summary>
//    /// 根据重生类型获取玩家出生坐标
//    /// </summary>
//    private Vector3 GetNewPlayerPosition(RespawnType type)
//    {
//        // 传送门重生
//        if (type == RespawnType.Portal)
//        {
//            Object_Portal portal = Object_Portal.instnace;
//            Vector3 pos = portal.GetPosition();
//            portal.SetTrigger(false);
//            portal.DisableIfNeeded();
//            return pos;
//        }

//        // 通用重生：优先最近已解锁存档点/场景入口
//        if (type == RespawnType.NonSpecific)
//        {
//            GameData saveData = SaveManager.instance.GetGameData();

//            // 筛选已解锁存档点
//            //var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None)
//             //   .Where(cp => saveData.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
//             //   .Select(cp => cp.GetPosition());

//            // 筛选场景入口路点
//            var enterPoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
//                .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
//                .Select(wp => wp.GetPositionAndSetTriggerFalse());

//            // 合并位置列表
//            //var allSpawnPoints = checkpoints.Concat(enterPoints).ToList();
//            //if (allSpawnPoints.Count == 0)
//             //   return Vector3.zero;

//            // 选取距离上一玩家位置最近的点
//            //return allSpawnPoints
//               // .OrderBy(pos => Vector3.Distance(pos, lastPlayerPosition))
//               // .First();
//        }

//        // 按指定路点类型获取位置
//        return GetWaypointPosition(type);
//    }

//    /// <summary>
//    /// 根据重生类型匹配对应路点位置
//    /// </summary>
//    private Vector3 GetWaypointPosition(RespawnType type)
//    {
//        Object_Waypoint[] waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);
//        foreach (var point in waypoints)
//        {
//            if (point.GetWaypointType() == type)
//                return point.GetPositionAndSetTriggerFalse();
//        }
//        return Vector3.zero;
//    }

//    /// <summary>
//    /// 读取存档数据
//    /// </summary>
//    public void LoadData(GameData data)
//    {
//        lastScenePlayed = data.lastScenePlayed;
//        lastPlayerPosition = data.lastPlayerPosition;

//        // 兜底默认场景
//        if (string.IsNullOrEmpty(lastScenePlayed))
//            lastScenePlayed = "Level_0";

//        dataLoaded = true;
//    }

//    /// <summary>
//    /// 保存运行数据至存档
//    /// </summary>
//    public void SaveData(ref GameData data)
//    {
//        string currentScene = SceneManager.GetActiveScene().name;
//        // 主菜单不记录游戏进度
//        if (currentScene == "MainMenu")
//            return;

//        data.lastPlayerPosition = Player.instance.transform.position;
//        data.lastScenePlayed = currentScene;
//        dataLoaded = false;
//    }
////}