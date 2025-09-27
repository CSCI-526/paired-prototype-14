using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;

    private int score = 0;
    private float elapsedTime = 0f;
    public float pointsPerSecond = 1f;
    private bool isPlaying = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (isPlaying)
        {
            
            elapsedTime += pointsPerSecond * Time.deltaTime;
            score = Mathf.FloorToInt(elapsedTime);
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ShowFinalScore(TextMeshProUGUI finalScoreText)  
    {
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;
    }


    public void StopScoring()
    {
        isPlaying = false;
    }

    public int GetScore() => score;
}
