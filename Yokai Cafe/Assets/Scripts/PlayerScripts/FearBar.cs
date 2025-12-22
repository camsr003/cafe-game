using UnityEngine;
using UnityEngine.UI;

public class FearBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image BgImage;
    [SerializeField] private PlayerController player;

    void Update()
    {
        if (!player) return;

        float normalized = player.CurrentFear / player.MaxFear;
        fillImage.fillAmount = normalized;

        fillImage.enabled = normalized > 0;
        BgImage.enabled = normalized > 0;
    }
}
