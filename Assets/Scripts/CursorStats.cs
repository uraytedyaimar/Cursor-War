using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStats : MonoBehaviour
{
    public static CursorStats Instance { get; private set; }

    [Header("Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;

    /* Soon
    [SerializeField] private float luck;
    [SerializeField] private float armor;
    [SerializeField] private float magnet;
    [SerializeField] private float reroll;
    [SerializeField] private float recovery;
    [SerializeField] private float cooldown;
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalChance;
    */

    public List<PlayerStatUpgrade> statUpgrades;
    [System.Serializable]
    public class PlayerStatUpgrade {
        public StatUpgrade statUpgrade;
        public int currentLevel;

        public string GetAbilityName() {
            return statUpgrade.statName;
        }

        public Sprite GetAbilityIcon() {
            return statUpgrade.statIcon;
        }

        public string GetAbilityDescription() {
            return statUpgrade.description;
        }

        public void Execute() {
            var config = statUpgrade.GetStatUpgradeConfig(currentLevel);
            if (config != null) {
                float effectAmount = config.effectAmount;

                switch (statUpgrade.statType) {
                    case StatType.MaxHp:
                        CursorStats.Instance.IncreaseMaxHp(effectAmount);
                        break;
                    case StatType.Damage:
                        CursorStats.Instance.IncreaseDamage(effectAmount);
                        break;
                    case StatType.Range:
                        CursorStats.Instance.IncreaseRange(effectAmount);
                        break;
                    case StatType.AttackSpeed:
                        CursorStats.Instance.IncreaseAttackSpeed(effectAmount);
                        break;
                        // Tambahkan case lainnya jika diperlukan
                }
            }
        }

        public bool CanLevelUp() {
            return currentLevel < statUpgrade.maxLevel;
        }

        public void LevelUp() {
            currentLevel++;
        }
    }

    private void Awake() {
        Instance = this;
        hp = maxHp;
    }

    public void UpdateCurrentHealth(float amount) {
        hp = amount;
    }

    public float GetMaxHp() {
        return maxHp;
    }

    public float GetRange() {
        return range;
    }

    public void IncreaseMaxHp(float amount) {
        maxHp += amount;
        hp = MathF.Min(hp + amount, maxHp);
        Debug.Log("Max HP increased by " + amount);
    }

    public void IncreaseDamage(float amount) {
        damage += (damage * amount);
        Debug.Log("Damage increased by " + amount);
    }

    public void IncreaseRange(float amount) {
        range += (range * amount);
        Debug.Log("Attack Speed increased by " + amount);
    }

    public void IncreaseAttackSpeed(float amount) {
        attackSpeed += (attackSpeed * amount);
        Debug.Log("Range increased by " + amount);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
