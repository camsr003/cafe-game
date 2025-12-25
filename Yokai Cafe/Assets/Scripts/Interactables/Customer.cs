using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour, IInteractable
{
    [Header("Customer Data")]
    public CustomerData customerData; // assign this in Inspector

    public string InteractName =>
        customerData != null ? customerData.customerName : "Customer";

    public string InteractPrompt => "Interact";


    private float waitTimer;
    private bool isLeaving = false;

    public bool isServed = false;

    [Header("Queue")]
    public Transform queuePosition;

    [Header("Order Data")]
    public OrderData currentOrder;

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
                WaitInQueue();
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
            transform.position += dir.normalized * customerData.moveSpeed * Time.deltaTime;
            transform.forward = dir.normalized;
        }
        else
        {
            state = CustomerState.Waiting;
            waitTimer = 0f;
        }
    }

    void WaitInQueue()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer >= customerData.maxWaitTime)
        {
            Debug.Log($"{customerData.customerName} got tired of waiting and left!");
            state = CustomerState.Leaving;
        }
    }


    void Leave()
    {
        // Destroy after some secs
        if (isLeaving) return;
        isLeaving = true;
        StartCoroutine(LeaveRoutine());
    }

    private IEnumerator LeaveRoutine()
    {
        yield return new WaitForSeconds(1f);
        QueueManager.Instance.RemoveCustomer(this);
        Destroy(gameObject);
    }

    public void SetQueuePosition(Transform queue)
    {
        queuePosition = queue;
        state = CustomerState.GoingToQueue;
    }

    public void AssignOrder(OrderData order)
    {
        currentOrder = order;
    }

    public void Interact()
    {
        if (state == CustomerState.Waiting && !isServed)
        {
            HoldableItem held = PlayerHoldController.Instance.GetHeldItem();
            if (held == null)
            {
                Debug.Log($"Customer wants {currentOrder.orderName}");
                return;
            }

            if (held.itemData != null && held.itemData.itemName == currentOrder.orderName) // correct order
            {
                Destroy(held.gameObject);
                isServed = true;
                DayManager.Instance.earnings += customerData.reward;
                DayManager.Instance.activeCustomers.Remove(this);
                state = CustomerState.Leaving;
                Debug.Log($"{customerData.customerName} has been served and is leaving!");
            }
            else
            {
                Debug.Log($" Wrong order. Customer wants {currentOrder.orderName}");
            }
        }
    }
}
