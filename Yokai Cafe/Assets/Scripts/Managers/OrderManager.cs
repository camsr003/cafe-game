using UnityEngine;
public class OrderManager : MonoBehaviour
{
    public OrderData[] possibleOrders;

    public OrderData GetRandomOrder()
    {
        return possibleOrders[Random.Range(0, possibleOrders.Length)];
    }
}
