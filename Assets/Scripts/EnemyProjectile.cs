using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody2D rb;

    private float bulletDamage;
    private float bulletSpeed;
    private Vector2 dir;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        MoveBullet(dir);
    }

    public static void Create(Vector3 origin, Vector3 dir, float speed, float damage) {
        Transform bulletTransform = Instantiate(GameAsset.Instance.enemyProjectilePrefab, origin, Quaternion.identity);
        EnemyProjectile bullet = bulletTransform.GetComponent<EnemyProjectile>();

        Vector3 newDir = (dir - origin).normalized;
        bullet.dir = newDir;
        bullet.bulletDamage = damage;
        bullet.bulletSpeed = speed;
    }

    private void MoveBullet(Vector3 dir) {
        rb.velocity = dir * bulletSpeed;
    }

    private void Start() {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Mouse mouse = collision.GetComponent<Mouse>();
        if (mouse != null) {
            mouse.Damage(bulletDamage);
        }
    }
}
