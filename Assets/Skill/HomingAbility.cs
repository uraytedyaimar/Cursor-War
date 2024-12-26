using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAbility : MonoBehaviour
{
    private Rigidbody2D rb;

    private float damage;
    private float speed = 15f;
    private float destroyCooldown;
    private Vector3 dynamicDirection;

    private Enemy enemy;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        DestroySelf();
    }

    private void Update() {
        if (enemy != null) {
            dynamicDirection = (enemy.GetPosition() - transform.position).normalized;
        }
        
        // float angle = Mathf.Atan2(dynamicDirection.y, dynamicDirection.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate() {
        rb.velocity = dynamicDirection * speed;
    }

    public static void Create(GameObject projectilePrefab, Vector3 playerPosition, Enemy enemy, float damage, float destroyCooldown) {
        GameObject homing = Instantiate(projectilePrefab, playerPosition, Quaternion.identity);
        HomingAbility homingAbility = homing.GetComponent<HomingAbility>();

        homingAbility.enemy = enemy;
        homingAbility.damage = damage;
        homingAbility.destroyCooldown = destroyCooldown;
    }

    private void DestroySelf() {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(damage);
        }
    }

    /*
     * dynamicDirection = (enemy.transform.position - transform.position).normalized;

        
    */
}
