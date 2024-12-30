using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : AbilityBase
{
    // random enemy
    // instant, timer
    // high damage

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private bool readyToDamage;
    private float readyToDamageTimer;
    private float readyToDamageTimerMax = 0.1f;

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    private void Update() {
        if (!readyToDamage) {
            readyToDamageTimer += Time.deltaTime;
            if (readyToDamageTimer >= readyToDamageTimerMax) {
                readyToDamage = true;
                readyToDamageTimer = 0f;
            }
        }
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        for (int i = 0; i < abilityConfig.prefabAmount; i++) {
            Enemy enemy = Enemy.GetRandomEnemyInRange();
            if (enemy != null) {
                GameObject prefab = Instantiate(ability.abilityPrefab, enemy.GetPosition(), Quaternion.identity);
                Electric electric = prefab.GetComponent<Electric>();

                electric.Setup(ability, abilityConfig);
            }
        }
    }

    private void Setup(Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (readyToDamage) {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Damage(abilityConfig.effectAmount);
                enemy.ApplySolidTint(new Color(1, 1, 0, 1));
                enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                readyToDamage = false;
            }
        }
    }
}
