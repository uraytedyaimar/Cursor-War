using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsUI;

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowCredits() {
        creditsUI.SetActive(true);
    }
    
    public void ExitCredits() {
        creditsUI.SetActive(false);

    }

    public void ExitGame() {
        Application.Quit();
    }
}
