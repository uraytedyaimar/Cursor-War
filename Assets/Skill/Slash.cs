using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : AbilityBase
{
    // closest enemy
    // piercing
    // high damage

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        GameObject prefab = Instantiate(ability.abilityPrefab, playerTransform.position, Quaternion.identity);
        Slash slash = prefab.GetComponent<Slash>();

        slash.Setup(playerTransform, ability, abilityConfig);
    }

    private void Setup(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;
        this.gameObject.transform.parent = playerTransform;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(abilityConfig.effectAmount);
            enemy.ApplySolidTint(new Color(1, 0, 0, 1));
            enemy.ApplyKnockback(transform.position);
        }
    }
}
