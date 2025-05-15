using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private string gameSceneName = "1"; // Coloca aqui o nome da tua cena do jogo

    private void Start()
    {
        // Garante que o botão está ligado ao método
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        Debug.Log("A iniciar o jogo...");
        SceneManager.LoadScene(gameSceneName);
    }
}