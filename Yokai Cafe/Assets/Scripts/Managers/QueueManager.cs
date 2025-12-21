using UnityEngine;
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    public static QueueManager Instance;

    [Header("Queue Positions")]
    public Transform[] queuePositions;

    private List<Customer> customersInQueue = new List<Customer>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddCustomer(Customer customer)
    {
        customersInQueue.Add(customer);
        UpdateQueuePositions();
    }

    public void RemoveCustomer(Customer customer)
    {
        customersInQueue.Remove(customer);
        DayManager.Instance.activeCustomers.Remove(customer);
        UpdateQueuePositions();
    }

    private void UpdateQueuePositions()
    {
        for (int i = 0; i < customersInQueue.Count; i++)
        {
            customersInQueue[i].SetQueuePosition(queuePositions[i]);
        }
    }
}
