using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private Transform expPrefab;
    [SerializeField] private Transform deadParticlePrefab;
    private Rigidbody2D rb;
    private static GameObject target;
    private HealthSystem healthSystem;
    private MaterialTintColor materialTintColor;
    private Camera mainCamera;

    [Header("Stats")]
    [SerializeField] private float maxHp;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float moveSpeed;
    private float hp;
    private float originalMoveSpeed;

    // Shoot
    private float shootTimer;
    private float shootTimerMax = 1f;

    // Knockback
    [Header("Knockback")]
    [SerializeField] private float knockbackStrength = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;
    private bool isKnockedBack;
    private float knockbackTimer;
    private Vector2 knockbackDirection;

    // Slow
    private bool isSlowed;
    private float slowSpeed;
    private float slowTimer;
    private float slowTimerMax = 2.5f;

    // Boss
    [SerializeField] private bool isBos;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        healthSystem = new HealthSystem(maxHp);
        materialTintColor = GetComponent<MaterialTintColor>();
        mainCamera = Camera.main;
        SetHealthSystem();

        enemyList.Add(this);
        hp = maxHp;
        shootTimer = shootTimerMax;
    }

    private static List<Enemy> enemyList = new List<Enemy>();
    private static List<Enemy> enemyInRangeList = new List<Enemy>();
    private List<Enemy> GetEnemyList() {
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
        int randomIndex = UnityEngine.Random.Range(0, enemyInRangeList.Count);
        return enemyInRangeList[randomIndex];
    }


    private void Start() {
        if (Mouse.Instance != null) {
            target = Mouse.Instance.gameObject;
        } else {
            target = FindObjectOfType<Mouse>().gameObject;
        }

        originalMoveSpeed = moveSpeed;
        slowSpeed = moveSpeed / 2;
    }

    private void UpdateEnemiesInRange() {
        enemyInRangeList.Clear();

        foreach (Enemy enemy in enemyList) {
            Vector3 enemyPosition = Camera.main.WorldToViewportPoint(enemy.GetPosition());
            if (IsInRange(enemyPosition)) {
                enemyInRangeList.Add(enemy);
            }
        }
    }

    private bool IsInRange(Vector3 viewportPosition) {
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z > 0;
    }

    private void Update() {
        if (isSlowed) {
            slowTimer += Time.deltaTime;
            moveSpeed = slowSpeed;
            ApplySolidTint(new Color(0, 1, 1, 1));
            if (slowTimer >= slowTimerMax) {
                isSlowed = false;
                slowTimer = 0f;
                moveSpeed = originalMoveSpeed;
            }
        }

        if (isKnockedBack) {
            if (isBos) return;
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0) {
                isKnockedBack = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void FixedUpdate() {
        UpdateEnemiesInRange();

        if (isKnockedBack) {
            rb.velocity = knockbackDirection * knockbackStrength;
        } else {
            HandleMovement();
        }
    }

    // Basic
    private void HandleMovement() {
        Vector3 targetPosition = target.transform.position - transform.position;
        rb.velocity = targetPosition.normalized * moveSpeed;

        float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg - 135f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void Attack() {
        EnemyProjectile.Create(attackPosition.position, target.transform.position, attackSpeed, damage);
    }
    public void Damage(float damage) {
        healthSystem.Damage(damage);
    }
    public Vector3 GetPosition() {
        return transform.position;
    }

    // Effects
    public void ApplySolidTint(Color color) {
        materialTintColor.SetTintColor(color);
    }
    public void ApplyKnockback(Vector3 projectilePosition) {
        if (isBos) return;

        knockbackDirection = (transform.position - projectilePosition).normalized;
        knockbackTimer = knockbackDuration;
        isKnockedBack = true;

        rb.velocity = Vector2.zero;
    }
    public void ApplySlowDebuff() {
        if (isBos) return;

        isSlowed = true;
    }

    // Health System
    private void SetHealthSystem() {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnDead += HealthSystem_OnDead;
    }
    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        hp = healthSystem.GetHealth();
    }
    private void HealthSystem_OnDead(object sender, EventArgs e) {
        Die();

        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        healthSystem.OnDead -= HealthSystem_OnDead;
    }
    private void Die() {
        enemyList.Remove(this);
        enemyInRangeList.Remove(this);
        Destroy(gameObject);
        Instantiate(deadParticlePrefab, transform.position, Quaternion.identity);
        Instantiate(expPrefab, transform.position, Quaternion.identity);

    }

    // Testing
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
