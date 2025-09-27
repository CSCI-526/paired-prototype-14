using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;          
    public TextMeshProUGUI titleText;         // "Game Over!"
    public TextMeshProUGUI finalScoreText;    // "Final Score: X"

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false); 
    }

    public void TriggerGameOver()
    {
        
        ScoreManager.Instance.StopScoring();

        
        if (titleText != null)
            titleText.text = "Game Over!";

        
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.GetScore();

        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
