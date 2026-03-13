// ========================================================
// 作者：娇娇 
// 创建时间：2025-12-28 17:34:18
// 版本：V1.1
// 描述：随机攻击特效
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private bool autoDestroy = true;// 是否可自动销毁
    [SerializeField] private float destroyDelay = 1;// 延迟销毁
    [Space]
    [SerializeField] private bool randomOffset = true;// 是否随即偏差
    [SerializeField] private bool randomRotation = true;// 是否随机旋转
    [Header("Fade effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;




    [Header("")]
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;


    [Header("随机位置偏差")]
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [Space]
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
            StartCoroutine(FadeCo());

        ApplyRandomOffset();
        ApplyRandomRotation();

        if(autoDestroy)
            Destroy(gameObject,destroyDelay);
    }
    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;

        while(targetColor.a >0)
        {
            targetColor.a = targetColor.a - (fadeSpeed * Time.deltaTime);
            sr.color = targetColor;
            yield return null;
        }
        sr.color = targetColor;
    }
    /// <summary>
    /// 位置随机偏差
    /// </summary>
    private void ApplyRandomOffset()
    {
        if (randomOffset == false)
            return;

        float xOffset = Random.Range(xMinOffset,xMaxOffset);
        float yOffset = Random.Range(yMinOffset,yMaxOffset);

        transform.position = transform.position + new Vector3(xOffset, yOffset);
    }
    /// <summary>
    /// 角度随机旋转
    /// </summary>
    private void ApplyRandomRotation()
    {
        if(randomRotation == false)
            return;

        float zRotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0,0,zRotation);
    }

}
