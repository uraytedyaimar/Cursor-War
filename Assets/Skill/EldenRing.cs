using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldenRing : AbilityBase
{
    // player position
    // orbit, timer
    // low damage + burn debuff

    private Ability ability;
    private Ability.AbilityConfig abilityConfig;

    private float timer;
    private float timerMax = 0.1f;

    private void Start() {
        Destroy(gameObject, abilityConfig.destroyTimer);
    }

    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        GameObject prefab = Instantiate(ability.abilityPrefab, playerTransform.position, Quaternion.identity);
        EldenRing eldenRing = prefab.GetComponent<EldenRing>();

        eldenRing.Setup(playerTransform, ability, abilityConfig);
    }

    private void Setup(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        this.ability = ability;
        this.abilityConfig = abilityConfig;
        this.gameObject.transform.parent = playerTransform;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        timer += Time.deltaTime;
        if (timer >= timerMax) {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Damage(abilityConfig.effectAmount);
                enemy.ApplySolidTint(new Color(1, 0, 0, 1));
            }
            timer = 0f;
        }
    }
}
