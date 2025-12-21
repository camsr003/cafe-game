using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour, IInteractable
{
    public string customerName;
    public bool isServed = false;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Queue")]
    public Transform queuePosition;

    private enum CustomerState
    {
        GoingToQueue,
        Waiting,
        Leaving
    }

    private CustomerState state = CustomerState.GoingToQueue;

    void Update()
    {
        switch (state)
        {
            case CustomerState.GoingToQueue:
                MoveToQueue();
                break;
            case CustomerState.Leaving:
                Leave();
                break;
            case CustomerState.Waiting:
                // Do nothing
                break;
        }
    }

    void MoveToQueue()
    {
        if (queuePosition == null) return;

        Vector3 targetPos = queuePosition.position;
        targetPos.y = transform.position.y; // keep on ground

        Vector3 dir = targetPos - transform.position;
        if (dir.magnitude > 0.05f)
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;
            transform.forward = dir.normalized;
        }
        else
        {
            state = CustomerState.Waiting;
        }
    }

    void Leave()
    {
        // Destroy after some secs
        StartCoroutine(LeaveRoutine());
    }

    private IEnumerator LeaveRoutine()
    {
        yield return new WaitForSeconds(2f);
        QueueManager.Instance.RemoveCustomer(this);
        Destroy(gameObject);
    }

    public void SetQueuePosition(Transform queue)
    {
        queuePosition = queue;
        state = CustomerState.GoingToQueue;
    }

    public void Interact()
    {
        if (state == CustomerState.Waiting && !isServed)
        {
            isServed = true;
            DayManager.Instance.earnings += 100;
            state = CustomerState.Leaving;
            Debug.Log($"{customerName} has been served and is leaving!");
        }
        else
        {
            Debug.Log("Customer is leaving.");
        }
    }
}
