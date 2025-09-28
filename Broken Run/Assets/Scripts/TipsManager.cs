using UnityEngine;
using TMPro;
using System.Collections;

public class TipsManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI tipText;   
    private CanvasGroup canvasGroup;

    [Header("Tips Settings")]
    [TextArea]
    public string[] tips = {
        "When the block color changes, your controls are reversed!", 
        "Press UP Arrow to jump over obstacles!",
        "Keep moving, don’t get caught by the killer!",
        "High score tip: Stay calm and time your jumps!",
        "Practice makes perfect. Keep trying!",
        "Watch out for the Killer, don’t get too close!"
    };

    public float tipInterval = 5f;     
    public float fadeDuration = 1f;    
    public float showDuration = 3f;    
    public float initialDelay = 4f;    

    private int currentIndex = 0;

    void Start()
    {
        canvasGroup = tipText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = tipText.gameObject.AddComponent<CanvasGroup>();

        tipText.text = "";
        StartCoroutine(TipsLoop());
    }

    IEnumerator TipsLoop()
    {
        
        yield return new WaitForSecondsRealtime(initialDelay);

        
        yield return ShowTip(tips[0]);
        currentIndex = 1; 

        
        while (true)
        {
            yield return new WaitForSeconds(tipInterval);

            string tip = tips[currentIndex];
            currentIndex = (currentIndex + 1) % tips.Length;

            yield return ShowTip(tip);
        }
    }

    IEnumerator ShowTip(string message)
    {
        tipText.text = message;

        
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;

        
        yield return new WaitForSecondsRealtime(showDuration);

        
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            canvasGroup.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    
    public void ShowImmediateTip(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowTip(message));
        StartCoroutine(TipsLoop()); 
    }
}
