using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private TimerScript timer;
    private bool firstCross = true;

    private void Start()
    {
        timer = FindFirstObjectByType<TimerScript>();
        if (timer == null)
            Debug.LogError("Não encontrei o TimerScript na cena!");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Só reagir ao carro (assume que o carro tem a tag "Player")
        if (!other.CompareTag("Player")) return;

         if (firstCross)
        {
            // Primeira vez que cruza: arranca o cronómetro
            timer.ResetTimer();
            timer.StartTimer();
            firstCross = false;
            Debug.Log("Lap started");
        }
        else
        {
            // Volta concluída: pára o cronómetro e regista o tempo
            timer.StopTimer();
            float lapTime = timer.GetElapsedTime();
            Debug.Log($"Lap finished: {lapTime:F2} s");

            LapManager.Instance.RecordLap(lapTime);


            // Se quiseres permitir múltiplas voltas, podes fazer:
            timer.ResetTimer();
            timer.StartTimer();
        }
    }
}
