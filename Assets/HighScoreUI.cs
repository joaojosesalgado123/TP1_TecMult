using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreUI : MonoBehaviour
{
    public static HighScoreUI Instance { get; private set; }

    [SerializeField] private RectTransform container;
    [SerializeField] private TextMeshProUGUI entryPrefab;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
       
        panel.SetActive(false); 
    }

    private void Start()
    {
       
    }

    public void ShowTable(List<float> times)
    {
        panel.SetActive(true);
        UpdateTable(times);
    }

    public void UpdateTable(List<float> times)
    {
        // Limpar entradas antigas
        foreach (Transform child in container) Destroy(child.gameObject);

        // Criar nova lista
        for (int i = 0; i < times.Count; i++)
        {
            if (times[i] == float.MaxValue) break;
            var e = Instantiate(entryPrefab, container);
            e.text = $"{i+1}. {FormatTime(times[i])}";
        }
    }

    private string FormatTime(float t)
    {
        int m = Mathf.FloorToInt(t / 60f);
        int s = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 1000f) % 1000f);
        return $"{m:00}:{s:00}:{ms:000}";
    }
}
