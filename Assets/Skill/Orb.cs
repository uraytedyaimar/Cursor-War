using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : AbilityBase
{
    // closest enemy
    // destroy after hit
    // high damage

    private Rigidbody2D rb;

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private float speed;
    private Vector3 constantDirection;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        speed = abilityConfig.speed;
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        Enemy enemy = Enemy.GetClosestEnemyInRange(playerTransform.position);
        if (enemy != null) {
            GameObject prefab = Instantiate(ability.abilityPrefab, playerTransform.position, Quaternion.identity);
            Orb orb = prefab.GetComponent<Orb>();

            orb.Setup(enemy, ability, abilityConfig);
        }
    }

    private void Setup(Enemy enemy, Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;

        this.constantDirection = (enemy.GetPosition() - transform.position).normalized;
        float angle = Mathf.Atan2(constantDirection.y, constantDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate() {
        rb.velocity = constantDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(abilityConfig.effectAmount);
            enemy.ApplyKnockback();
            Destroy(gameObject);
        }
    }
}