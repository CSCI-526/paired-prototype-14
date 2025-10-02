using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;   // Assign your Slider in Inspector
    public Text healthText;       // Optional: assign your "HealthText" object here

    private float maxHealth = 100f;

    void Awake()
    {
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>(); // Auto-grab if not set
    }

    void Start()
    {
        ResetHealth();
    }

    public void SetHealth(float value)
    {
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Clamp(value, 0f, maxHealth);

            if (healthText != null)
                healthText.text = $"{Mathf.RoundToInt(healthSlider.value)}";
        }
    }

    public void ResetHealth()
    {
        SetHealth(maxHealth);
    }
}