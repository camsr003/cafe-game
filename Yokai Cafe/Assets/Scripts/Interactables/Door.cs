using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public Transform hinge;
    public float openAngle = 90f;
    public float speed = 4f;

    private bool isOpen;
    private float closedAngle;
    private float targetAngle;

    void Start()
    {
        // Store the door's actual closed rotation
        closedAngle = hinge.localEulerAngles.y;
        targetAngle = closedAngle;
    }

    void Update()
    {
        float current = hinge.localEulerAngles.y;
        float next = Mathf.LerpAngle(current, targetAngle, Time.deltaTime * speed);
        hinge.localEulerAngles = new Vector3(0, next, 0);
    }

    public void Interact()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;

            Vector3 toPlayer = (player.position - hinge.position).normalized;
            float dot = Vector3.Dot(hinge.forward, toPlayer);

            // Open away from the player, relative to closed angle
            float direction = dot > 0 ? -1f : 1f;
            targetAngle = closedAngle + openAngle * direction;
        }
        else
        {
            // Return to original rotation
            targetAngle = closedAngle;
        }
    }
}
