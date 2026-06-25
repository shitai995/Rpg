// ========================================================
// 作者：娇娇 
// 创建时间：2026-05-22 12:14:01
// 版本：V1.1
// 描述：全局游戏存档数据实体类，存储金币、背包、装备、技能、场景等存档信息
// ========================================================

using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// 游戏全局存档数据
/// </summary>
[Serializable]
public class GameData
{
    public int gold; // 金币数量

    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory;       // 背包：物品ID -> 堆叠数量
    public SerializableDictionary<string, int> storageItems;    // 仓库道具
    public SerializableDictionary<string, int> storageMaterials;// 仓库材料

    public SerializableDictionary<string, ItemType> equipedItems;// 已装备物品：物品ID -> 装备槽位

    public int skillPoints; // 技能点数
    public SerializableDictionary<string, bool> skillTreeUI;                // 技能解锁状态
    public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades;// 技能升级类型

    public SerializableDictionary<string, bool> unlockedCheckpoints; // 已解锁存档点
    public SerializableDictionary<string, Vector3> inScenePortals;  // 场景传送门位置

    public SerializableDictionary<string, bool> completedQuests; // 已完成任务
    public SerializableDictionary<string, int> activeQuests;    // 当前进行中任务

    public string portalDestinationSceneName; // 传送目标场景名
    public bool returningFromTown;            // 是否从城镇返回

    public string lastScenePlayed;    // 上次所在场景
    public Vector3 lastPlayerPosition;// 玩家上次坐标

    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();

        equipedItems = new SerializableDictionary<string, ItemType>();

        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();

        unlockedCheckpoints = new SerializableDictionary<string, bool>();
        inScenePortals = new SerializableDictionary<string, Vector3>();

        completedQuests = new SerializableDictionary<string, bool>();
        activeQuests = new SerializableDictionary<string, int>();
    }
}