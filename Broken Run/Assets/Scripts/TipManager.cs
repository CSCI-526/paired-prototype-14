using UnityEngine;
using TMPro;
using System.Collections;

public class TipManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI tipText;   
    private CanvasGroup canvasGroup;

    [Header("Tips Settings")]
    [Tooltip("If empty, default English tips will be used")]
    public string[] tips;   // Inspector 填写可覆盖默认值

    public float tipInterval = 8f;   // seconds between tips
    public float fadeDuration = 1f;  // fade in/out time

    private void Start()
{
    if (tipText != null)
    {
        canvasGroup = tipText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = tipText.gameObject.AddComponent<CanvasGroup>();

        
        if (tips == null || tips.Length == 0 || string.IsNullOrWhiteSpace(tips[0]))
        {
            tips = new string[] {
                
                "Keep moving, don’t get caught by the left boundary!",
                "High score tip: Stay calm and time your jumps!",
                "Practice makes perfect. Keep trying!",
                "Watch out for the Killer, don’t get too close!"
            };
        }

        
        int index = Random.Range(0, tips.Length);
        tipText.text = tips[index];
        canvasGroup.alpha = 1;

        
        StartCoroutine(ShowRandomTips());
    }
}


    private IEnumerator ShowRandomTips()
    {
        while (true)
        {
            yield return new WaitForSeconds(tipInterval);

            if (tipText != null && tips.Length > 0)
            {
                int index = Random.Range(0, tips.Length);
                yield return StartCoroutine(FadeText(index));
            }
        }
    }

    private IEnumerator FadeText(int tipIndex)
    {
        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;

        // Change text
        tipText.text = tips[tipIndex];

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}
