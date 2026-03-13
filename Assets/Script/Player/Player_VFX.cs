// ========================================================
// 作者：娇娇 
// 创建时间：2026-03-09 17:28:32
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;
using System.Collections;
using UnityEditor;
public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(.01f, .2f)]
    [SerializeField] private float imageEchoInterval = .05f;// 残影生成间隔
    [SerializeField] private GameObject imageEchoPrefab;// 残影预制体
    private Coroutine imageEchoCo;

    public void DoImageEchoEffect(float duration)
    {
        if(imageEchoCo != null)
            StopCoroutine(imageEchoCo);

        imageEchoCo = StartCoroutine(IamgeEchoEffectCo(duration));
    }

    private IEnumerator IamgeEchoEffectCo(float duration)
    {
        float timeTracker = 0;

        while(timeTracker < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);
            timeTracker = timeTracker + imageEchoInterval;
        }
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
