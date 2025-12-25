using UnityEngine;
using System.Collections.Generic;

public class TrashCan : MonoBehaviour, IInteractable
{
    [Header("Capacity")]
    [SerializeField] private int maxCapacity = 5;
    [SerializeField] private int currentTrash = 0;

    [Header("Restrictions")]
    [SerializeField] private List<string> nonTrashableItems;

    [Header("Trash Bag")]
    [SerializeField] private HoldableItem trashBagPrefab;
    [SerializeField] private Transform bagSpawnPoint;

    private bool hasSpawnedBag = false;
    public string InteractName => "Trash Can";

    public string InteractPrompt
    {
        get
        {
            if (IsFull)
                return "Trash Can Full";

            if (!PlayerHoldController.Instance.IsHoldingItem())
                return "Nothing to Throw Away";

            HoldableItem held = PlayerHoldController.Instance.GetHeldItem();

            if (held.itemData != null &&
                nonTrashableItems.Contains(held.itemData.itemName))
                return "Cannot Trash This";

            return "Throw Away";
        }
    }

    private bool IsFull => currentTrash >= maxCapacity;

    public void Interact()
    {
        if (IsFull)
        {
            OnTrashCanFull();
            return;
        }

        HoldableItem held = PlayerHoldController.Instance.GetHeldItem();

        if (held == null)
            return;

        if (held.itemData != null &&
            nonTrashableItems.Contains(held.itemData.itemName))
            return;

        // Destroy item
        Destroy(held.gameObject);
        PlayerHoldController.Instance.ClearHeldItem();

        currentTrash++;

        if (IsFull)
            OnTrashCanFull();
    }

    private void OnTrashCanFull()
    {
        if (hasSpawnedBag)
            return;

        hasSpawnedBag = true;

        Transform spawnPoint = bagSpawnPoint != null
            ? bagSpawnPoint
            : transform;

        Instantiate(
            trashBagPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        currentTrash = 0;

        Debug.Log("Trash bag spawned!");
    }
}
