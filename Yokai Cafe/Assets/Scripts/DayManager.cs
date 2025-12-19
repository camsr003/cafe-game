using UnityEngine;
using System.Collections;

public class DayManager : MonoBehaviour
{
    [Header("Day Settings")]
    public float prepareDuration = 5f;
    public float serveDuration = 8f;
    public float maintainDuration = 5f;
    public float spiritEventDuration = 5f;
    public float closeDuration = 5f;

    [Header("Sun & Night Settings")]
    public Light mainLight;
    public float morningX = 50f;  // sun high in sky
    public float noonX = 30f;
    public float eveningX = 5f;
    public float nightX = -10f;    // below horizon

    public float morningIntensity = 1f;
    public float noonIntensity = 1f;
    public float eveningIntensity = 0.3f;
    public float nightIntensity = 0f; // night = sun off


    void SetSun(float xRotation, float intensity)
    {
        mainLight.transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        mainLight.intensity = intensity;
    }

    void Start()
    {
        StartCoroutine(DayRoutine());
    }

    IEnumerator DayRoutine()
    {
        yield return Prepare();
        yield return Serve();
        yield return Maintain();
        yield return Close();
        yield return SpiritEvent();

        Debug.Log("Day cycle complete! You can restart or move to next day.");
        // Optionally restart automatically
        StartCoroutine(DayRoutine());
    }

    IEnumerator Prepare()
    {
        Debug.Log("Preparing...");
        //Set up cafe, flip open sign?
        SetSun(morningX, morningIntensity);
        yield return new WaitForSeconds(prepareDuration);
    }

    IEnumerator Serve()
    {
        Debug.Log("Serving Customers...");
        // Serve and Maintain
        SetSun(noonX, noonIntensity);
        yield return new WaitForSeconds(serveDuration);
    }

    IEnumerator Maintain()
    {
        Debug.Log("Maintaining Cafe...");
        //Prepare for close
        SetSun(eveningX, eveningIntensity);
        yield return new WaitForSeconds(maintainDuration);
    }

    IEnumerator Close()
    {
        Debug.Log("Closing Cafe...");
        SetSun(nightX, nightIntensity);
        // flip close sign, prepare for close
        yield return new WaitForSeconds(closeDuration);
    }

    IEnumerator SpiritEvent()
    {
        Debug.Log("Spirit Event Occurring...");
        // survive then leave
        yield return new WaitForSeconds(spiritEventDuration);
    }
}
