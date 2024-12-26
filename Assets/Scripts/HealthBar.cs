using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;
    private float damagedHealthShrinkTimer;

    [SerializeField] private Image barImage;
    [SerializeField] private Image damagedBarImage;

    private HealthSystem healthSystem;

    public void Set(HealthSystem healthSystem) { 
        this.healthSystem = healthSystem;

        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void Start() {
        UpdateHealthBar();
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    private void Update() {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0) {
            if (barImage.fillAmount < damagedBarImage.fillAmount) {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        barImage.fillAmount = healthSystem.GetHealthNormalized();
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
    }
}
