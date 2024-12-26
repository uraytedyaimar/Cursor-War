using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class LevelSystemAnimated
{
    public event EventHandler OnLevelChanged;
    public event EventHandler OnExperienceChanged;

    private LevelSystem levelSystem;
    private bool isAnimating;
    private float updateTimer;
    private float updateTimerMax;

    private int level;
    private int experience;

    [SerializeField] private LevelUpWindow levelUpWindow;

    public LevelSystemAnimated(LevelSystem levelSystem) {
        SetLevelSystem(levelSystem);
        updateTimerMax = 0.016f;

        FunctionUpdater.Create(() => Update());
    }

    public void SetLevelSystem(LevelSystem levelSystem) { 
        this.levelSystem = levelSystem;

        level = levelSystem.GetLevel();
        experience = levelSystem.GetExperience();

        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e) {
        isAnimating = true;
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e) {
        isAnimating = true;
    }

    private void Update() {
        if (isAnimating) {
            updateTimer += Time.deltaTime;
            while (updateTimer > updateTimerMax) {
                updateTimer -= updateTimerMax;

                UpdateAddExperience();
            }
        }
    }

    private void UpdateAddExperience() {
        if (level < levelSystem.GetLevel()) {
            AddExperience();
        } else {
            if (experience < levelSystem.GetExperience()) {
                AddExperience();
            } else {
                isAnimating = false;
            }
        }
    }

    private void AddExperience() {
        // experience++;
        // belum berhasil karena exp bernilai int dan yang ingin ditambah bernilai float
        experience += (levelSystem.GetExperienceToNextLevel(level) / 100);

        if (experience >= levelSystem.GetExperienceToNextLevel(level)) {
            level++;
            experience = 0;
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetLevel() {
        return level;
    }

    public float GetExperienceNormalized() {
        return (float) experience / levelSystem.GetExperienceToNextLevel(level);
    }
}
