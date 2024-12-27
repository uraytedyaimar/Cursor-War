using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {

    [Header("Reference")]
    private Rigidbody2D rb;
    private static GameObject target;
    private HealthSystem healthSystem;
    private MaterialTintColor materialTintColor;
    private Camera mainCamera;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private Transform expPrefab;
    [SerializeField] private Transform deadParticlePrefab;

    [Header("Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;

    // Other
    private float shootTimer;
    private float shootTimerMax = 1f;
    private float followingDelay = 0f;
    private float followingDelayMax;

    private float knockbackStrength = 5f;
    private float knockbackDuration = 0.2f;
    private bool isKnockedBack = false;
    private float knockbackTimer;
    private Vector2 knockbackDirection;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        healthSystem = new HealthSystem(maxHp);
        materialTintColor = GetComponent<MaterialTintColor>();
        mainCamera = Camera.main;
        enemyList.Add(this);
        SetHealthSystem();
        hp = maxHp;
        shootTimer = shootTimerMax;
        followingDelayMax = UnityEngine.Random.Range(0.1f, 0.5f);
        followingDelay = followingDelayMax;
    }

    private static List<Enemy> enemyList = new List<Enemy>();
    private static List<Enemy> enemyInRangeList = new List<Enemy>();

    public static List<Enemy> GetEnemyList() {
        return enemyList;
    }

    public static List<Enemy> GetEnemyInRangeList() {
        return enemyInRangeList;
    }

    public static Enemy GetClosestEnemyInRange(Vector3 position) {
        if (enemyInRangeList == null || enemyInRangeList.Count == 0) return null;
        Enemy closestEnemy = null;
        for (int i = 0; i < enemyInRangeList.Count; i++) {
            Enemy testEnemy = enemyInRangeList[i];

            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(enemyInRangeList[i].GetPosition());
            // Periksa apakah posisi musuh berada di dalam viewport
            if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z > 0) {

                if (closestEnemy == null) {
                    closestEnemy = testEnemy;
                } else {
                    if (Vector3.Distance(position, testEnemy.GetPosition()) < Vector3.Distance(position, closestEnemy.GetPosition())) {
                        closestEnemy = testEnemy;
                    }
                }
            }
        }

        return closestEnemy;
    }

    public static Enemy GetRandomEnemyInRange() {
        if (enemyInRangeList == null || enemyInRangeList.Count == 0) return null;

        // Pilih musuh secara acak dari daftar musuh dalam kamera
        int randomIndex = UnityEngine.Random.Range(0, enemyInRangeList.Count);
        return enemyInRangeList[randomIndex];
    }


    private void Start() {
        if (Mouse.Instance != null) {
            target = Mouse.Instance.gameObject;
        } else {
            target = FindObjectOfType<Mouse>().gameObject;  // Fallback to player
            Debug.LogWarning("Mouse.Instance is null. Defaulting to player.");
        }
    }

    private void Update() {
        // Cek apakah musuh sedang terkena knockback
        if (isKnockedBack) {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0) {
                isKnockedBack = false; // Knockback selesai
                rb.velocity = Vector2.zero;
            }
        } else {
            // Musuh mengejar target
            MoveTowardsTarget();
        }

        foreach (Enemy enemy in enemyList) {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(enemy.GetPosition());

            // Periksa apakah posisi musuh berada di dalam viewport
            if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z > 0) {
                enemyInRangeList.Add(enemy);
            } else {
                enemyInRangeList.Remove(enemy);
            }
        }
    }

    private void FixedUpdate() {
        if (isKnockedBack) {
            // Terapkan knockback dengan menggunakan velocity
            rb.velocity = knockbackDirection * knockbackStrength; // Gunakan velocity untuk knockback
        } else {
            // Jika tidak terkena knockback, pergerakan normal
            Vector2 targetPosition = Vector2.MoveTowards(rb.position, target.transform.position, moveSpeed * Time.fixedDeltaTime);
            rb.velocity = (targetPosition - rb.position).normalized * moveSpeed; // Menggunakan velocity untuk pergerakan normal
        }
    }

    private void MoveTowardsTarget() {
        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToTarget < range) {
            followingDelay = 0f;
            shootTimer += Time.deltaTime;
            if (shootTimer > shootTimerMax) {
                shootTimer = 0f;
                Attack();
            }
        } else {
            followingDelay += Time.deltaTime;
            if (followingDelay > followingDelayMax) {
                Vector2 targetPosition = Vector2.MoveTowards(rb.position, target.transform.position, moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(targetPosition);
            }
        }

        Vector3 targetDir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 135f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Attack() {
        EnemyProjectile.Create(attackPosition.position, target.transform.position, attackSpeed, damage);
    }

    public void Damage(float damage) {
        healthSystem.Damage(damage);
    }

    private void Die() {
        enemyList.Remove(this);
        Destroy(gameObject);
        Instantiate(deadParticlePrefab, transform.position, Quaternion.identity);
        Instantiate(expPrefab, transform.position, Quaternion.identity);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void SetHealthSystem() {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        hp = healthSystem.GetHealth();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e) {
        ApplyKnockback();
        ApplySolidTint();
    }

    private void ApplyKnockback() {
        knockbackDirection = (transform.position - target.transform.position).normalized;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;

        rb.velocity = Vector2.zero;
    }

    private void ApplySolidTint() {
        materialTintColor.SetTintColor(new Color(1, 1, 1, 1f));
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e) {
        // Boss ???
    }

    private void HealthSystem_OnDead(object sender, EventArgs e) {
        Die();

        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnHealed -= HealthSystem_OnHealed;
        healthSystem.OnDead -= HealthSystem_OnDead;
    }
}
