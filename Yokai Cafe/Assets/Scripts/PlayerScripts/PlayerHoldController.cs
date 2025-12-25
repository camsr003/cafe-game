using UnityEngine;

public class PlayerHoldController : MonoBehaviour
{
    public static PlayerHoldController Instance { get; private set; }

    public Transform holdPoint;
    private HoldableItem heldItem;
    private float lastDropTime;
    public float dropCooldown = 0.25f;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsHoldingItem() => heldItem != null;

    public HoldableItem GetHeldItem()
    {
        return heldItem;
    }

    public void ClearHeldItem()
    {
        heldItem = null;
    }


    public void HoldItem(HoldableItem item)
    {
        if (heldItem != null) return;

        heldItem = item;
        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
        if (item.TryGetComponent<Collider>(out var col)) col.enabled = false;
    }

    public void DropItem()
    {
        if (heldItem == null) return;

        heldItem.transform.SetParent(null);

        if (heldItem.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = false;
        if (heldItem.TryGetComponent<Collider>(out var col)) col.enabled = true;

        heldItem = null;
    }
}
