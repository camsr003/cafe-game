using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    public string customerName;
    public float moveSpeed = 2f;
    public float maxWaitTime = 10f;
    public int reward = 100;
    // Optional: color, prefab reference, mood type, etc.
}
