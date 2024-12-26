using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite abilityIcon;
    public GameObject abilityPrefab;
    public AbilityType abilityType;
    public int maxLevel = 5;

    public List<AbilityConfig> levels;
    [System.Serializable]
    public class AbilityConfig {
        public string description;
        public int prefabAmount;
        public float effectAmount;
        public float cooldown;
        public float destroyTimer;
    }

    public AbilityConfig GetAbilityConfig(int level) {
        if (level >= 1 && level <= levels.Count) {
            return levels[level - 1];
        }
        return null;
    }
}

public enum AbilityType {
    Melee,
    Projectile,
    Homing,
    Instant,
    Orbit,
    AreaOfEffect,
}
