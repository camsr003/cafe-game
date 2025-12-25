using UnityEngine;

[CreateAssetMenu(menuName = "Orders/Order")]
public class OrderData : ScriptableObject
{
    public string orderName;

    [Header("Requirements")]
    public ItemData[] requiredItems;   // or ingredients
    public int[] requiredAmounts;

    [Header("Rewards")]
    public int moneyReward;
    public float patienceDrainModifier = 1f;
}
