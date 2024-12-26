using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image experienceBarImage;

    public void Set(LevelSystem levelSystem, LevelSystemAnimated levelSystemAnimated) {
        this.levelSystem = levelSystem;
        this.levelSystemAnimated = levelSystemAnimated;

        SetLevelNumber(levelSystemAnimated.GetLevel());
        SetExperienceBarImage(levelSystemAnimated.GetExperienceNormalized());

        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e) {
        SetLevelNumber(levelSystemAnimated.GetLevel());
    }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e) {
        SetExperienceBarImage(levelSystemAnimated.GetExperienceNormalized());
    }

    private void SetLevelNumber(int levelNumber) {
        levelText.text = "Lv. " + levelNumber;
    }

    private void SetExperienceBarImage(float experienceNormalized) {
        experienceBarImage.fillAmount = experienceNormalized;
    }
}
