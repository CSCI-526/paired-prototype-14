using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;   
    public static ScoreManager Instance; 

    [Header("Score Settings")]
    public float scoreRate = 10f; 
    private float score = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isGameOver) return;

        score += Time.deltaTime * scoreRate;
        scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
    }

    public void GameOver()
    {
        isGameOver = true;

        int finalScore = GetFinalScore();
        SaveScore(finalScore);
    }

    public int GetFinalScore()
    {
        return Mathf.FloorToInt(score);
    }

    
    private void SaveScore(int newScore)
    {
        List<int> scores = new List<int>();

        
        for (int i = 0; i < 5; i++)
        {
            scores.Add(PlayerPrefs.GetInt("HighScore" + i, 0));
        }

        
        scores.Add(newScore);

        
        scores.Sort((a, b) => b.CompareTo(a));

        
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, scores[i]);
        }
    }

    
    public List<int> GetTopScores()
    {
        List<int> topScores = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            topScores.Add(PlayerPrefs.GetInt("HighScore" + i, 0));
        }
        return topScores;
    }

    public void AddScore(int amount)
{
    if (isGameOver) return;

    score += amount;
    scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
}

}
