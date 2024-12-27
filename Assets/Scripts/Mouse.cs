using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    public static Mouse Instance { get; private set; }

    [Header("Reference")]
    private Rigidbody2D rb;
    private CursorStats cursorStats;
    private AbilityManager abilityManager;
    private HealthSystem healthSystem;
    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private LevelWindow levelWindow;
    [SerializeField] private LevelUpWindow levelUpWindow;

    // Other
    private bool isDead = false;
    private Vector3 mousePos;
    
    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

        cursorStats = GetComponent<CursorStats>();
        abilityManager = GetComponent<AbilityManager>();

        healthSystem = new HealthSystem(cursorStats.GetMaxHp());
        levelSystem = new LevelSystem();
        levelSystemAnimated = new LevelSystemAnimated(levelSystem);

        healthBar.Set(healthSystem);
        levelWindow.Set(levelSystem, levelSystemAnimated);
        levelUpWindow.Set(cursorStats, levelSystem, levelSystemAnimated, abilityManager);

        SetHealthSystem();
    }

    private void Update() {
        SetMousePosition();
    }

    private void SetMousePosition() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    public Vector3 GetPosition() { 
        return transform.position;
    }

    public void Damage(float damage) {
        healthSystem.Damage(damage);
    }

    public bool IsDead() {
        return isDead;
    }

    private void SetHealthSystem() {
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDead += HealthSystem_OnDead;
    }
    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        // Update current health
        cursorStats.UpdateCurrentHealth(healthSystem.GetHealth());
    }
    private void HealthSystem_OnDamaged(object sender, EventArgs e) {
        // nanti diatur lagi agar shake nya lebih baik
        CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
    }
    private void HealthSystem_OnDead(object sender, EventArgs e) {
        // ubah cursor nya menjadi ikon tengkorak atau mati
        isDead = true;

        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnDead -= HealthSystem_OnDead;
    }

    public void AddExperience(int amount) {
        levelSystem.AddExperience(amount);
    }
}