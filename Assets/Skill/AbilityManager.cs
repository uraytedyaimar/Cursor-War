using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<PlayerAbility> unlockedAbilities;
    public List<PlayerAbility> lockedAbilities;

    [System.Serializable]
    public class PlayerAbility {
        public Ability ability;
        public int currentLevel = 1;
        private float abilityCooldown;

        public string GetAbilityName() {
            return ability.abilityName;
        }

        public Sprite GetAbilityIcon() {
            return ability.abilityIcon;
        }

        public string GetAbilityDescription() {
            return ability.GetAbilityConfig(currentLevel)?.description ?? "No description";
        }

        public void UpdateCooldown() {
            if (abilityCooldown > 0) { 
                abilityCooldown -= Time.deltaTime;
            }
        }

        public void Activate(Transform myTransform) {
            if (abilityCooldown <= 0f) {
                Execute(myTransform);
                abilityCooldown = ability.GetAbilityConfig(currentLevel).cooldown;
            }
        }

        public void Execute(Transform myTransform) {
            var config = ability.GetAbilityConfig(currentLevel);
            if (config != null && ability.abilityPrefab != null) {

                IAbility instance = ability.abilityPrefab.GetComponent<IAbility>();
                if (instance != null) {
                    instance.Create(myTransform, ability, config);
                }
            }
        }

        public bool CanLevelUp() {
            return currentLevel < ability.maxLevel;
        }

        public void LevelUp() {
            currentLevel++;
        }
    }

    private void Update() {
        foreach (var ability in unlockedAbilities) { 
            ability.UpdateCooldown();
        }

        foreach (var ability in unlockedAbilities) {
            // hanya activate ketika ada musuh dalam range camera
            if (Enemy.GetEnemyInRangeList().Count > 0) {
                ability.Activate(transform);
            }
        }
    }
}
