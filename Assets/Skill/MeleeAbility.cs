using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : AbilityBase
{
    // closest enemy
    // piercing
    // damage

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        GameObject prefab = Instantiate(ability.abilityPrefab, playerTransform.position, Quaternion.identity);
        MeleeAbility meleeAbility = prefab.GetComponent<MeleeAbility>();

        meleeAbility.Setup(ability, abilityConfig);
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
