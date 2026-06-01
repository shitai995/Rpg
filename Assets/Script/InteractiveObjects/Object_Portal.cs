// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:29:15
// 版本：V1.1
// 描述：传送门逻辑，支持场景传送与数据存档
// ========================================================

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 传送门对象，实现场景传送及存档功能
/// </summary>
public class Object_Portal : MonoBehaviour, ISaveable
{
    public static Object_Portal instnace;

    public bool isActive { get; private set; }
    [SerializeField] private Vector2 defaultPosition;    // 城镇内默认生成位置
    [SerializeField] private string townSceneName = "Level_0"; // 城镇场景名

    [SerializeField] private Transform respawnPoint;     // 传送重生点
    [SerializeField] private bool canBeTriggered;         // 是否可触发传送

    private string currentSceneName;
    private string returnSceneName;
    private bool returningFromTown;

    private void Awake()
    {
        instnace = this;
        currentSceneName = SceneManager.GetActiveScene().name;
        transform.position = new Vector3(9999, 9999); // 初始隐藏传送门
    }

    /// <summary>
    /// 激活传送门
    /// </summary>
    public void ActivatePortal(Vector3 position, int facingDir = 1)
    {
        isActive = true;
        transform.position = position;
        SaveManager.instance.GetGameData().inScenePortals.Clear();

        // 调整朝向
        if (facingDir == -1)
            transform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// 按需关闭传送门
    /// </summary>
    public void DisableIfNeeded()
    {
        if (!returningFromTown)
            return;

        SaveManager.instance.GetGameData().inScenePortals.Remove(currentSceneName);
        isActive = false;
        transform.position = new Vector3(9999, 9999);
    }

    /// <summary>
    /// 执行场景传送
    /// </summary>
    private void UseTeleport()
    {
        string destinationScene = InTown() ? returnSceneName : townSceneName;
        //GameManager.instance.ChangeScene(destinationScene, RespawnType.Portal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeTriggered) return;
        UseTeleport();
    }

    private void OnTriggerExit2D(Collider2D collision) => canBeTriggered = true;

    /// <summary>
    /// 设置传送触发状态
    /// </summary>
    public void SetTrigger(bool trigger) => canBeTriggered = trigger;

    /// <summary>
    /// 获取传送点位置
    /// </summary>
    public Vector3 GetPosition() => respawnPoint != null ? respawnPoint.position : transform.position;

    /// <summary>
    /// 判断当前是否在城镇场景
    /// </summary>
    private bool InTown() => currentSceneName == townSceneName;

    #region 存档接口
    /// <summary>
    /// 读取存档数据
    /// </summary>
    public void LoadData(GameData data)
    {
        // 城镇场景
        if (InTown() && data.inScenePortals.Count > 0)
        {
            transform.position = defaultPosition;
            isActive = true;
        }
        // 其他场景
        else if (data.inScenePortals.TryGetValue(currentSceneName, out Vector3 portalPosition))
        {
            transform.position = portalPosition;
            isActive = true;
        }

        returningFromTown = data.returningFromTown;
        returnSceneName = data.portalDestinationSceneName;
    }

    /// <summary>
    /// 保存当前数据
    /// </summary>
    public void SaveData(ref GameData data)
    {
        data.returningFromTown = InTown();

        if (isActive && !InTown())
        {
            data.inScenePortals[currentSceneName] = transform.position;
            data.portalDestinationSceneName = currentSceneName;
        }
        else
        {
            data.inScenePortals.Remove(currentSceneName);
        }
    }
    #endregion
}