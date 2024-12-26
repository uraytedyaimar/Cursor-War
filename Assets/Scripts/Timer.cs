using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI gameOvertimer;

    private float seconds;

    private void Update() {
        // hanya update kalau belum game over
        if (Mouse.Instance.IsDead()) {
            return;
        }

        // Tambahkan deltaTime ke seconds
        seconds += Time.deltaTime;

        // Hitung menit dan detik
        int minutes = Mathf.FloorToInt(seconds / 60f); // Konversi detik ke menit
        int remainingSeconds = Mathf.FloorToInt(seconds % 60f); // Sisa detik

        // Format menit dan detik menjadi string
        string minutesString = minutes.ToString("0");
        string secondsString = remainingSeconds.ToString("00");

        // Update teks timer
        timer.text = minutesString + ":" + secondsString;
        gameOvertimer.text = "Timer : " + minutesString + ":" + secondsString;
    }
}
