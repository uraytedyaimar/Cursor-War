using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private float health;
    private float healthMax;

    public HealthSystem(float healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public float GetHealth() {
        return health;
    }

    public float GetHealthMax() {
        return healthMax;
    }

    public float GetHealthNormalized() {
        return health / healthMax;
    }

    public void Damage(float damage) { 
        health -= damage;

        if (health < 0) { 
            health = 0;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead() {
        return health <= 0;
    }

    public void Heal(float heal) { 
        health += heal;
        if (health > healthMax) {
            health = healthMax;
        }

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealComplete(float heal) {
        health = healthMax;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealthMax(float healthMax, bool fullHealth) {
        this.healthMax = healthMax;
        if (fullHealth) {
            health = healthMax;
        }
        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetHealth(float health) {
        if (health > healthMax) {
            health = healthMax;
        }
        if (health < 0) {
            health = 0;
        }
        this.health = health;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if (health <= 0) {
            Die();
        }
    }
}
