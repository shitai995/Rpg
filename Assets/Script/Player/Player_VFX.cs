// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 17:28:32
// 版本：V1.1
// 描述：玩家专属特效管理类，继承实体特效基类，新增残影特效
// ========================================================

using UnityEngine;
using System.Collections;
using UnityEditor;

public class Player_VFX : Entity_VFX
{
    [Header("角色残影特效")]
    [Range(.01f, .2f)]
    [Tooltip("残影生成时间间隔")]
    [SerializeField] private float imageEchoInterval = .05f;

    [Tooltip("残影特效预制体")]
    [SerializeField] private GameObject imageEchoPrefab;

    private Coroutine imageEchoCo;

    /// <summary>
    /// 通用特效生成
    /// </summary>
    public void CreateEffectOf(GameObject effect, Transform target)
    {
        Instantiate(effect, target.position, Quaternion.identity);
    }

    /// <summary>
    /// 开启残影拖影效果
    /// </summary>
    public void DoImageEchoEffect(float duration)
    {
        if (imageEchoCo != null)
            StopCoroutine(imageEchoCo);
        imageEchoCo = StartCoroutine(IamgeEchoEffectCo(duration));
    }

    /// <summary>
    /// 残影持续生成协程
    /// </summary>
    private IEnumerator IamgeEchoEffectCo(float duration)
    {
        float timeTracker = 0;
        while (timeTracker < duration)
        {
            CreateImageEcho();
            yield return new WaitForSeconds(imageEchoInterval);
            timeTracker += imageEchoInterval;
        }
    }

    /// <summary>
    /// 生成单帧角色残影
    /// </summary>
    private void CreateImageEcho()
    {
        GameObject echoObj = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        echoObj.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}