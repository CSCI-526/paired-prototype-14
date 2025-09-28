using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI countdownText; 
    public float countdownTime = 3f;      

    void Start()
    {
        
        Time.timeScale = 0f;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        float remaining = countdownTime;

        while (remaining > 0)
        {
            countdownText.text = Mathf.CeilToInt(remaining).ToString();
            yield return new WaitForSecondsRealtime(1f); 
            remaining--;
        }

        
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(1f);

        
        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
