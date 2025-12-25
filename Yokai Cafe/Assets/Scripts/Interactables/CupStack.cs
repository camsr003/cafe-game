using UnityEngine;

public class CupStack : MonoBehaviour, IInteractable
{
    [Header("Cup Settings")]
    [SerializeField] private HoldableItem cupPrefab;

    public string InteractName => "Cup Stack";
    public string InteractPrompt =>
        PlayerHoldController.Instance.IsHoldingItem()
            ? "Hands Full"
            : "Pick Up Empty Cup";

    public void Interact()
    {
        // If already holding something, do nothing
        if (PlayerHoldController.Instance.IsHoldingItem())
            return;

        // Spawn cup at hold point
        HoldableItem cup = Instantiate(
            cupPrefab,
            PlayerHoldController.Instance.holdPoint.position,
            Quaternion.identity
        );

        // Make player hold the cup
        PlayerHoldController.Instance.HoldItem(cup);
    }


}
