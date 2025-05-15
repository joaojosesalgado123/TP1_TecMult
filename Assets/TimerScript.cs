using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime;
    private bool isRunning = true;

    void Start()
    {
        elapsedTime = 0f;
        isRunning = false;    
        timerText.text = "00:00";
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;

            // Converte o tempo para minutos e segundos
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // Atualiza o texto do Timer
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public bool IsRunning => isRunning;

    public float GetElapsedTime() => elapsedTime;

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    
    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerText.text = "00:00";
    }

    
}