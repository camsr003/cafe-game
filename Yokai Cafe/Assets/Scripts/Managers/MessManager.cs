using UnityEngine;

public class MessManager : MonoBehaviour
{
    [Header("Mess Settings")]
    public MessData[] messTypes;       // ScriptableObjects for different messes
    public Transform[] spawnPoints;
    public int maxMesses = 2;         // Max active messes
    public float spawnInterval = 5f;   // Time between spawns

    private float timer = 0f;
    private int activeMesses = 0;

    private void Update()
    {
        // Only spawn when Serving state
        if (DayManager.Instance.CurrentState != DayState.Serving)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval && activeMesses < maxMesses)
        {
            SpawnMess();
            timer = 0f;
        }
    }

    void SpawnMess()
    {
        // Choose a random mess type
        int messIndex = Random.Range(0, messTypes.Length);
        MessData selectedMess = messTypes[messIndex];

        Vector3 spawnPos;

        // Choose position: either predefined or random
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        spawnPos = spawnPoints[spawnIndex].position;

        // Instantiate mess prefab
        GameObject messGO = Instantiate(selectedMess.messPrefab, spawnPos, Quaternion.identity);
        messGO.GetComponent<Mess>().messData = selectedMess;

        activeMesses++;
        messGO.GetComponent<Mess>().OnCleaned += () => activeMesses--; // Optional event callback
    }
}
