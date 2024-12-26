using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatUpgrade", menuName = "StatUpgrade")]
public class StatUpgrade : ScriptableObject
{
    public string statName;
    public StatType statType;
    public Sprite statIcon;
    public string description;
    public int maxLevel = 5;

    public List<StatUpgradeConfig> levels;
    [System.Serializable]
    public class StatUpgradeConfig {
        public float effectAmount;
    }

    public StatUpgradeConfig GetStatUpgradeConfig(int level) {
        if (level >= 1 && level <= levels.Count) {
            return levels[level - 1];
        }
        return null;
    }
}

public enum StatType {
    MaxHp,
    Damage,
    Range,
    AttackSpeed,
}
