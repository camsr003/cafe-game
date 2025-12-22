using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image BgImage;
    [SerializeField] private PlayerController player;

    void Update()
    {
        if (!player) return;

        float normalized = player.CurrentHealth / player.MaxHealth;
        fillImage.fillAmount = normalized;

        fillImage.enabled = normalized < 1f;
        BgImage.enabled = normalized < 1f;
    }
}
