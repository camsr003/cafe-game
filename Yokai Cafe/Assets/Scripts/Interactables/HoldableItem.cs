using UnityEngine;

public class HoldableItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (PlayerHoldController.Instance.IsHoldingItem())
            return;

        PlayerHoldController.Instance.HoldItem(this);
    }

    public void OnPickedUp(Transform holdPoint)
    {
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDropped()
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        col.enabled = true;
    }
}
