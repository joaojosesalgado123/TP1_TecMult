using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    public static LapManager Instance { get; private set; }

    private List<float> lapTimes = new List<float>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void RecordLap(float time)
    {
        lapTimes.Add(time);
        lapTimes.Sort();           // tempos do menor para o maior

        SaveBestTimes();
        HighScoreUI.Instance.UpdateTable(lapTimes);
    }

    private void SaveBestTimes()
    {
        // Guarda só os 5 melhores, por exemplo
        for (int i = 0; i < Mathf.Min(5, lapTimes.Count); i++)
            PlayerPrefs.SetFloat($"BestLap{i}", lapTimes[i]);
        PlayerPrefs.Save();
    }

    public List<float> GetBestTimes(int count = 5)
    {
        List<float> best = new List<float>();
        for (int i = 0; i < count; i++)
            best.Add(PlayerPrefs.GetFloat($"BestLap{i}", float.MaxValue));
        return best;
    }
}
