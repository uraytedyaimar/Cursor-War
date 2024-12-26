using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour {
    public static GameAsset Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public Transform enemyPrefab;
    public Transform enemyProjectilePrefab;
}
