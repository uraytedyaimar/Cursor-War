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

    private float timer;
    private float timerMax = 0.25f;

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        Enemy enemy = Enemy.GetRandomEnemyInRange();
        if (enemy != null) {
            GameObject prefab = Instantiate(ability.abilityPrefab, enemy.GetPosition(), Quaternion.identity);
            Electric electric = prefab.GetComponent<Electric>();

            electric.Setup(ability, abilityConfig);
        }
    }

    private void Setup(Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        timer += Time.deltaTime;
        if (timer >= timerMax) {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Damage(abilityConfig.effectAmount);
                enemy.ApplySolidTint(new Color(1, 1, 0, 1));
                enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            timer = 0f;
        }
    }
}
