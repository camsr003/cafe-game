using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public enum DayState
{
    Preparing,
    Serving,
    Closing,
    Review
}

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    public Transform player;
    public Transform playerSpawn;

    [Header("Sun & Lighting")]
    public Light mainLight;
    public float morningX = 50f;
    public float noonX = 30f;
    public float eveningX = 5f;
    public float nightX = -10f;

    public float morningIntensity = 1f;
    public float noonIntensity = 1f;
    public float eveningIntensity = 0.3f;
    public float nightIntensity = 0f;

    [Header("Serving Settings")]
    public Transform[] queuePositions;
    public GameObject customerPrefab;
    public int maxCustomers = 5;
    public int totalCustomers = 0;
    public List<Customer> activeCustomers = new List<Customer>();
    public float servingDuration = 60f;
    public List<Transform> entranceWaypoints;
    [Header("Progress")]
    private int currentDay = 0;
    public int earnings = 0;


    public DayState CurrentState { get; private set; }

    public bool HasCustomers()
    {
        return activeCustomers.Exists(c => !c.isServed);
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNextDay();
    }

    void SetSun(float xRotation, float intensity)
    {
        mainLight.transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        mainLight.intensity = intensity;
    }

    public void SetState(DayState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case DayState.Preparing:
                Debug.Log("Preparing");
                SetSun(morningX, morningIntensity);
                break;

            case DayState.Serving:
                Debug.Log("Serving");
                Time.timeScale = 1f;
                SetSun(noonX, noonIntensity);
                StartCoroutine(Serve());
                break;

            case DayState.Closing:
                Debug.Log("Closing");
                Time.timeScale = 1f;
                SetSun(nightX, nightIntensity);
                StartCoroutine(Close());
                break;

            case DayState.Review:
                Debug.Log("Review");
                ReviewManager.Instance.Show(currentDay, earnings);
                Time.timeScale = 0f;
                break;
        }
    }

    public void StartNextDay()
    {
        currentDay += 1;
        totalCustomers = 0;
        earnings = 0;
        maxCustomers = 5;

        //Prep cafe map
        //Close all doors
        

        // Move player to start
        SpawnPlayer.Instance.TeleportPlayer();

        SetState(DayState.Preparing);
    }

    private IEnumerator Close()
    {
        yield return new WaitForSeconds(3f);

        Debug.Log("Finished closing");
        SetState(DayState.Review);
    }

    private IEnumerator Serve()
    {
        float servingTime = servingDuration;

        // Start the spawning coroutine once
        Coroutine spawning = StartCoroutine(CustomerSpawningRoutine());

        while (servingTime > 0 || (totalCustomers < maxCustomers))
        {
            // Early exit if all customers are served
            if (activeCustomers.Count > 0 && activeCustomers.TrueForAll(c => c.isServed))
                break;

            servingTime -= Time.deltaTime;
            yield return null;
        }

        // Stop spawning when done
        StopCoroutine(spawning);
        Debug.Log("Can now close");
    }


    private IEnumerator CustomerSpawningRoutine()
    {
        yield return new WaitForSeconds(10f);
        while (true) // keep looping until manually stopped
        {
            if ((activeCustomers.Count < queuePositions.Length) && (totalCustomers < maxCustomers))
            {
                totalCustomers++;
                SpawnCustomer();
            }
            Debug.Log("Spawned");

            // Wait a random duration before spawning next
            float randomWaitDur = Random.Range(3f, 6f);
            yield return new WaitForSeconds(randomWaitDur);
        }
    }


    void SpawnCustomer()
    {
        // Spawn at a fixed location 
        Vector3 spawnPos = queuePositions[4].position;
        spawnPos.y += 0.1f; // small offset so they’re above ground

        GameObject newCustGO = Instantiate(customerPrefab, spawnPos, Quaternion.identity);
        Customer newCust = newCustGO.GetComponent<Customer>();

        activeCustomers.Add(newCust);
        QueueManager.Instance.AddCustomer(newCust);
    }

}
