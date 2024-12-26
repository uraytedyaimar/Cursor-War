using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : AbilityBase
{
    private Rigidbody2D rb;

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private float speed = 15f;
    private Vector3 constantDirection;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        GameObject prefab = Instantiate(ability.abilityPrefab, playerTransform.position, Quaternion.identity);
        ProjectileAbility projectileAbility = prefab.GetComponent<ProjectileAbility>();

        projectileAbility.Setup(ability, abilityConfig);
    }

    private void Setup(Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;

        // hanya activate ketika ada musuh dalam range camera
        Enemy enemy = GetClosestEnemyInRange();
        if (enemy != null) {
            constantDirection = (enemy.GetPosition() - transform.position).normalized;
            float angle = Mathf.Atan2(constantDirection.y, constantDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FixedUpdate() {
        rb.velocity = constantDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(abilityConfig.effectAmount);
            Destroy(gameObject);
        }
    }
}