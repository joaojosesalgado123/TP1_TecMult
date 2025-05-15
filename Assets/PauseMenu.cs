using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject HighScorePanel;
    private bool isPaused = false;

    void Start()
    {
        HighScorePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        HighScorePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void ShowHighScores()
    {
        // Preenche e mostra o painel de tempos
        List<float> best = LapManager.Instance.GetBestTimes();
        HighScoreUI.Instance.ShowTable(best);
        HighScorePanel.SetActive(true);
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}