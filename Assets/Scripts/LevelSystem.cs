using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public event EventHandler OnLevelChanged;
    public event EventHandler OnExperienceChanged;

    private int level;
    private int experience;
    private int experienceToNextLevel;

    public LevelSystem() {
        level = 1;
        experience = 0;
    }

    public void AddExperience(int amount) {
        experience += amount;
        while (experience >= GetExperienceToNextLevel(level)) {
            experience -= GetExperienceToNextLevel(level);
            level++;
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetLevel() {
        return level;
    }

    public int GetExperience() {
        return experience;
    }

    public float GetExperienceNormalized() {
        return (float)experience / GetExperienceToNextLevel(level);
    }

    public int GetExperienceToNextLevel(int level) {
        return (int)Mathf.Floor(100 * level * Mathf.Pow(level, 0.5f));
    }
}
