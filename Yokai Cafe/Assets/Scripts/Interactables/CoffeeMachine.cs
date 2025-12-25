using UnityEngine;
using System.Collections;

public class CoffeeMachine : MonoBehaviour, IInteractable
{
    [Header("Machine Settings")]
    public HoldableItem cupPrefab;

    public string InteractName => "Coffee Machine";

    public string InteractPrompt
    {
        get
        {
            HoldableItem held = PlayerHoldController.Instance.GetHeldItem();

            if (isReady)
                return "Take Coffee";

            if (isBrewing)
                return "Brewing...";

            if (held != null && held.itemData?.itemName == "Empty Cup")
                return "Place Empty Cup";

            return "Needs Empty Cup";
        }
    }

    private bool isBrewing = false;
    private bool isReady = false;
    public float brewTime = 3f;

    public void Interact()
    {
        // If coffee is ready, give cup
        if (isReady)
        {
            GiveCoffee();
            return;
        }

        // If already brewing do nothing
        if (isBrewing)
            return;

        // Otherwise try to start brewing
        TryStartBrewing();
    }

    private void TryStartBrewing()
    {
        HoldableItem held = PlayerHoldController.Instance.GetHeldItem();

        if (held == null)
            return;

        if (held.itemData == null || held.itemData.itemName != "Empty Cup")
            return;

        // Consume empty cup
        Destroy(held.gameObject);
        PlayerHoldController.Instance.ClearHeldItem();

        // Start brewing
        StartCoroutine(BrewCoffee());
    }

    private IEnumerator BrewCoffee()
    {
        isBrewing = true;

        // TODO: play brewing animation / sound here

        yield return new WaitForSeconds(brewTime);

        isBrewing = false;
        isReady = true;

        // TODO: visual indicator coffee is ready (light, steam, etc)
    }

    private void GiveCoffee()
    {
        // Must have free hands
        if (PlayerHoldController.Instance.GetHeldItem() != null)
            return;

        HoldableItem coffeeCup = Instantiate(
            cupPrefab,
            PlayerHoldController.Instance.holdPoint.position,
            Quaternion.identity
        );

        PlayerHoldController.Instance.HoldItem(coffeeCup);

        isReady = false;
    }
}
