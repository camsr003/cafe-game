using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public Transform hinge;
    public float openAngle = 90f;
    public float speed = 4f;

    private bool isOpen;
    private float targetAngle;

    void Update()
    {
        float current = hinge.localEulerAngles.y;
        float next = Mathf.LerpAngle(current, targetAngle, Time.deltaTime * speed);
        hinge.localEulerAngles = new Vector3(0, next, 0);
    }

    public void Interact()
    {
        isOpen = !isOpen;
        targetAngle = isOpen ? openAngle : 0f;
    }
}

