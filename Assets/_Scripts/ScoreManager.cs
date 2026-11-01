using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string format = "Score: {0}";

    public event Action<int> ScoreChanged;
    public int Score { get; private set; }

    private void OnEnable()
    {
        Enemy.Killed += AddScore;
    }

    private void OnDisable()
    {
        Enemy.Killed -= AddScore;
    }

    void Start()
    {
        UpdateText();
    }

    public void AddScore(int points)
    {
        if (points <= 0)
            return;

        Score += points;
        UpdateText();
        ScoreChanged?.Invoke(Score);
    }

    private void UpdateText()
    {
        if (scoreText != null)
            scoreText.text = string.Format(format, Score);
    }
}
