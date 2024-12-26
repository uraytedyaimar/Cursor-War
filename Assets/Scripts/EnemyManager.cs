using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float spawnTimer;
    private float difficulty = 1f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float InitialSpawnInterval = 3.5f;
    [SerializeField] private float incrementDifficulty = 0.02f;
    private float spawnInterval;

    private void Start() {
        spawnInterval = InitialSpawnInterval;
        SpawnEnemyBasedOnDifficulty();
    }

    private void Update() {
        spawnTimer += Time.deltaTime;
        difficulty += incrementDifficulty * Time.deltaTime;
        if (spawnTimer >= spawnInterval) {
            spawnTimer -= spawnInterval;
            spawnInterval = Mathf.Max(1f, spawnInterval - 0.01f);
            SpawnEnemyBasedOnDifficulty();
        }
    }

    private void SpawnEnemyBasedOnDifficulty() {
        int maxEnemyIndex = GetMaxEnemyIndex(difficulty); // Hitung jumlah jenis musuh yang bisa muncul
        int randomIndex = Random.Range(0, maxEnemyIndex + 1); // Pilih musuh secara acak dari yang tersedia
        Instantiate(enemyPrefabs[randomIndex], GetRandomPosition(), Quaternion.identity);
    }

    private int GetMaxEnemyIndex(float difficulty) {
        // Tentukan jumlah variasi musuh berdasarkan difficulty
        if (difficulty < 2) return 0; // Hanya Enemy 1
        if (difficulty < 4) return 1; // Enemy 1 dan 2
        if (difficulty < 6) return 2; // Enemy 1, 2, dan 3
        if (difficulty < 8) return 3; // Enemy 1, 2, 3, dan 4
        return 4;                     // Semua enemy tersedia
    }

    private Vector3 GetRandomPosition() {
        float randomX, randomY;

        // Tentukan apakah spawn di kiri/kanan atau atas/bawah
        bool spawnOnX = Random.value < 0.5f;  // Tentukan apakah spawn di kiri/kanan atau atas/bawah

        if (spawnOnX) {
            // Spawn di kiri atau kanan
            if (Random.value < 0.5f) {
                // Spawn di kiri layar
                randomX = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect - 5f, -Camera.main.orthographicSize * Camera.main.aspect - 2f);
            } else {
                // Spawn di kanan layar
                randomX = Random.Range(Camera.main.orthographicSize * Camera.main.aspect + 2f, Camera.main.orthographicSize * Camera.main.aspect + 5f);
            }

            // Spawn di posisi vertikal acak di dalam jangkauan atas/bawah kamera
            randomY = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
        } else {
            // Spawn di atas atau bawah
            if (Random.value < 0.5f) {
                // Spawn di bawah layar
                randomY = Random.Range(-Camera.main.orthographicSize - 5f, -Camera.main.orthographicSize - 2f);
            } else {
                // Spawn di atas layar
                randomY = Random.Range(Camera.main.orthographicSize + 2f, Camera.main.orthographicSize + 5f);
            }

            // Spawn di posisi horizontal acak di luar jangkauan kiri/kanan kamera
            randomX = Random.Range(-Camera.main.orthographicSize * Camera.main.aspect - 5f, Camera.main.orthographicSize * Camera.main.aspect + 5f);
        }

        return new Vector3(randomX, randomY, 0f); // Asumsi game 2D, tidak perlu z-coordinate
    }
}
