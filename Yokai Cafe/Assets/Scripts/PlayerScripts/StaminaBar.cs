using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image BgImage;
    [SerializeField] private PlayerController player;

    void Update()
    {
        if (!player) return;

        float normalized = player.CurrentStamina / player.MaxStamina;
        fillImage.fillAmount = normalized;

        fillImage.enabled = normalized < 1f;
        BgImage.enabled = normalized < 1f;
    }
}
