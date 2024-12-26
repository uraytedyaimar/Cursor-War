using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantAbility : MonoBehaviour
{
    private float damage;
    private float destroyCooldown;

    private void Start() {
        DestroySelf();
    }

    public static void Create(GameObject instantPrefab, Vector3 enemyPosition, float damage, float destroyCooldown) {
        GameObject instant = Instantiate(instantPrefab, enemyPosition, Quaternion.identity);
        InstantAbility instantAbility = instant.GetComponent<InstantAbility>();

        instantAbility.damage = damage;
        instantAbility.destroyCooldown = destroyCooldown;
    }

    private void DestroySelf() {
        Destroy(gameObject, destroyCooldown);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(damage);
        }
    }
}
