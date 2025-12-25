using UnityEngine;

public class Dumpster : MonoBehaviour, IInteractable
{
    public string InteractName => "Dumpster";
    public string InteractPrompt => "Throw Away Trash Bag";

    public void Interact()
    {
        HoldableItem held = PlayerHoldController.Instance.GetHeldItem();

        if (held == null)
            return;

        if (held.itemData == null || held.itemData.itemName != "Trash Bag")
            return;

        // Destroy item
        Destroy(held.gameObject);
        PlayerHoldController.Instance.ClearHeldItem();
    }
}
