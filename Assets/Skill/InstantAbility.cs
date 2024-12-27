using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantAbility : AbilityBase
{
    // random enemy
    // instant
    // damage

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        Enemy enemy = Enemy.GetRandomEnemyInRange();
        if (enemy != null) {
            GameObject prefab = Instantiate(ability.abilityPrefab, enemy.GetPosition(), Quaternion.identity);
            InstantAbility instantAbility = prefab.GetComponent<InstantAbility>();

            instantAbility.Setup(ability, abilityConfig);
        }
    }

    private void Setup(Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(abilityConfig.effectAmount);
        }
    }
}
