// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-29 22:30:00
// 版本：V1.1
// 描述：可序列化字典，解决Unity原生Dictionary无法序列化问题
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity可序列化泛型字典
/// </summary>
[System.Serializable]
public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    /// <summary>
    /// 反序列化完成后，从列表还原字典数据
    /// </summary>
    public void OnAfterDeserialize()
    {
        Clear();
        if (keys.Count != values.Count)
            Debug.LogWarning("键集合与值集合数量不匹配");

        for (int i = 0; i < keys.Count; i++)
        {
            this[keys[i]] = values[i];
        }
    }

    /// <summary>
    /// 序列化前，将字典数据转为列表存储
    /// </summary>
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}