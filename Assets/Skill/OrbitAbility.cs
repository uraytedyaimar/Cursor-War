using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAbility : AbilityBase {
    // player position
    // orbit
    // damage

    private Rigidbody2D rb;
    private Transform playerTransform;

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private float damage;
    private float angle;
    private float radius = 1.5f;
    private float speed = 360f;
    private float destroyTimer;

    public float followSpeed = 100f;
    private Vector3 currentCenter;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Destroy(gameObject, destroyTimer);
    }

    private void Update() {
        // Validasi playerTransform
        if (playerTransform == null) return;

        // Interpolasi pusat orbit agar mulus mengikuti pergerakan cursor
        currentCenter = Vector3.Lerp(currentCenter, playerTransform.position, followSpeed * Time.deltaTime);

        // Tambahkan kecepatan rotasi terhadap sudut
        angle += speed * Time.deltaTime;

        // Konversi sudut ke radian untuk fungsi trigonometri
        float radian = angle * Mathf.Deg2Rad;

        // Hitung posisi baru menggunakan sinus dan kosinus
        float x = currentCenter.x + Mathf.Cos(radian) * radius;
        float y = currentCenter.y + Mathf.Sin(radian) * radius;

        // Perbarui posisi objek
        transform.position = new Vector3(x, y, transform.position.z);
    }


    public static void Create(GameObject projectilePrefab, Transform playerTransform, float damage, float destroyTimer) {
        GameObject projectile = Instantiate(projectilePrefab, playerTransform.position, Quaternion.identity);
        OrbitAbility orbitAbility = projectile.GetComponent<OrbitAbility>();

        orbitAbility.playerTransform = playerTransform;
        orbitAbility.damage = damage;
        orbitAbility.destroyTimer = destroyTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(damage);
        }
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        throw new System.NotImplementedException();
    }
}
