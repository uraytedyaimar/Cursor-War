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

    [SerializeField] private float knockbackStrength = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;
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

    private void UpdateEnemiesInRange() {
        enemyInRangeList.Clear();

        foreach (Enemy enemy in enemyList) {
            Vector3 enemyPosition = Camera.main.WorldToViewportPoint(enemy.GetPosition());
            if (IsInRange(enemyPosition)) {
                enemyInRangeList.Add(enemy);
            } else {
                enemyInRangeList.Remove(enemy);
            }
        }
    }

    private bool IsInRange(Vector3 viewportPosition) {
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z > 0;
    }

    private void Update() {
        if (isKnockedBack) {
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

    private void Die() {
        enemyList.Remove(this);
        enemyInRangeList.Remove(this);
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
        ApplySolidTint();
    }

    public void ApplyKnockback() {
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
