using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private GameObject enemyManager;
    [SerializeField] private GameObject gameOverUI;

    private State state;
    private enum State {
        Playing,
        GameOver,
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        state = State.Playing;
    }

    private void Update() {
        switch (state) { 
            case State.Playing:
                if (Mouse.Instance.IsDead()) {
                    state = State.GameOver;
                }
                break;
            case State.GameOver:
                audioSource.enabled = false;
                AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position, 0.025f);
                StartCoroutine(GameOver());
                break;
        }
    }

    private IEnumerator GameOver() {
        yield return new WaitForSeconds(1.5f);

        enemyManager.SetActive(false);
        gameOverUI.SetActive(true);
    }
}
