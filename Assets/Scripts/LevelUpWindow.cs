using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour {
    private Mouse mouse;
    private CursorStats cursorStats;
    private AbilityManager abilityManager;
    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;

    private List<LevelUpChoice> finalChoices;
    private List<LevelUpChoice> possibleChoices;

    [SerializeField] private Transform choiceButton1;
    [SerializeField] private Transform choiceButton2;
    [SerializeField] private Transform choiceButton3;

    private void Awake() {
        finalChoices = new List<LevelUpChoice>();
        possibleChoices = new List<LevelUpChoice>();
    }

    private void Start() {
        Hide();
    }

    public void Set(CursorStats cursorStats, LevelSystem levelSystem, LevelSystemAnimated levelSystemAnimated, AbilityManager abilityManager) {
        this.cursorStats = cursorStats;
        this.levelSystem = levelSystem;
        this.levelSystemAnimated = levelSystemAnimated;
        this.abilityManager = abilityManager;

        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
    }

    private void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e) {
        GenerateLevelUpChoices();
        Show();
    }

    private void GenerateLevelUpChoices() {
        finalChoices.Clear();
        possibleChoices.Clear();

        // Kumpulkan semua stat upgrades
        if (cursorStats.statUpgrades != null) {
            foreach (var statUpgrade in cursorStats.statUpgrades) {
                possibleChoices.Add(new LevelUpChoice(
                    statUpgrade.GetAbilityName(),
                    statUpgrade.GetAbilityIcon(),
                    statUpgrade.currentLevel,
                    statUpgrade.GetAbilityDescription(),
                    () => {
                        statUpgrade.LevelUp(); // Aksi upgrade
                        statUpgrade.Execute();
                    }
                ));
            }
        }

        // Kumpulkan semua unlocked abilities
        if (abilityManager.unlockedAbilities != null) {
            foreach (var ability in abilityManager.unlockedAbilities) {
                possibleChoices.Add(new LevelUpChoice(
                    ability.GetAbilityName(),
                    ability.GetAbilityIcon(),
                    ability.currentLevel,
                    ability.GetAbilityDescription(),
                    () => ability.LevelUp() // Aksi upgrade
                ));
            }
        }

        // Kumpulkan semua locked abilities
        if (abilityManager.lockedAbilities != null) {
            foreach (var ability in abilityManager.lockedAbilities) {
                possibleChoices.Add(new LevelUpChoice(
                    ability.GetAbilityName(),
                    ability.GetAbilityIcon(),
                    0, // Level 0 karena belum dibuka
                    ability.GetAbilityDescription(),
                    () => UnlockAbility(ability)
                ));
            }
        }

        // Pilih 3 opsi secara acak
        for (int i = 0; i < 3; i++) {
            if (possibleChoices.Count > 0) {
                int randomIndex = UnityEngine.Random.Range(0, possibleChoices.Count);
                finalChoices.Add(possibleChoices[randomIndex]);
                possibleChoices.RemoveAt(randomIndex); // Hindari duplikasi
            }
        }

        UpdateUI();
    }

    private void UpdateUI() {
        // Validasi jumlah opsi
        if (finalChoices.Count < 3) {
            Debug.LogError("Not enough level-up choices to display!");
            return;
        }

        // Update UI untuk setiap tombol
        for (int i = 0; i < 3; i++) {
            LevelUpChoice choice = finalChoices[i];
            Transform choiceButton = i == 0 ? choiceButton1 : i == 1 ? choiceButton2 : choiceButton3;

            choiceButton.Find("Image").GetComponent<Image>().sprite = choice.Icon;
            choiceButton.Find("Name").GetComponent<TextMeshProUGUI>().text = choice.Name;
            choiceButton.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level: " + (choice.Level + 1);
            choiceButton.Find("Description").GetComponent<TextMeshProUGUI>().text = choice.Description;

            // Tambahkan aksi tombol
            Button button = choiceButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {
                choice.OnSelect();
                Hide();
            });
        }
    }

    private void UnlockAbility(AbilityManager.PlayerAbility ability) {
        abilityManager.lockedAbilities.Remove(ability);
        abilityManager.unlockedAbilities.Add(ability);
    }

    public void Show() {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause waktu
    }

    public void Hide() {
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Lanjut waktu
    }

    // Struktur data untuk pilihan level up
    private class LevelUpChoice {
        public Sprite Icon { get; }
        public string Name { get; }
        public int Level { get; }
        public string Description { get; }
        public Action OnSelect { get; }

        public LevelUpChoice(string name, Sprite icon, int level, string description, Action onSelect) {
            Name = name;
            Icon = icon;
            Level = level;
            Description = description;
            OnSelect = onSelect;
        }
    }
}
