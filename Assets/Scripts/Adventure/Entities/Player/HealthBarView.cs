// Player health bar: observes Health.HealthChanged and drives a filled image. (View)
using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        health.HealthChanged += OnHealthChanged;
        if (fillImage)
            fillImage.fillAmount = 1f;
    }

    private void OnDestroy()
    {
        if (health)
            health.HealthChanged -= OnHealthChanged;
    }
    
    private void OnHealthChanged(int current, int max)
    {
        if (fillImage)
            fillImage.fillAmount = max > 0 ? (float)current / max : 0f;
    }
}