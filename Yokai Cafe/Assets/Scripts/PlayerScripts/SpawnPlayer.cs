using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public static SpawnPlayer Instance;
    public CharacterController controller;
    public Transform playerSpawn;

    void Awake()
    {
        Instance = this;
    }
    public void TeleportPlayer()
    {
        // Temporarily disable controller to avoid interference
        controller.enabled = false;

        // Set position
        controller.transform.position = playerSpawn.position;

        // Re-enable controller
        controller.enabled = true;
    }
}
