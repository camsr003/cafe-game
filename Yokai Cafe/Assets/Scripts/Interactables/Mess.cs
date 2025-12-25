using UnityEngine;
using System;

public class Mess : MonoBehaviour, IInteractable
{
     public MessData messData;

    public string InteractName =>
        messData != null ? messData.messName : "Mess";

    public string InteractPrompt => "Clean";


    private bool isCleaned = false;
    public event Action OnCleaned;

    void TryClean()
    {
        // Get held item
        var heldItem = PlayerHoldController.Instance?.GetHeldItem();
        if (heldItem == null) return;

        // Check if it's a Mop
        if (heldItem.GetComponent<Mop>() == null) return;


        // Clean the mess
        Clean();
    }

    void Clean()
    {
        if (isCleaned) return;
        isCleaned = true;

        if (messData != null)
        {
            // if (messData.cleanEffect != null)
            //     Instantiate(messData.cleanEffect, transform.position, Quaternion.identity);

            // if (messData.cleanSound != null)
            //     AudioSource.PlayClipAtPoint(messData.cleanSound, transform.position);

            // GameManager.Instance.AddScore(messData.scoreValue);
        }
        
        OnCleaned?.Invoke(); // Notify manager
        Destroy(gameObject);
    }

    public void Interact()
    {
        if (isCleaned) return;

        TryClean();
    }
}
