using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public string interactName = "Light Switch";
    public string interactPrompt = "Interact";
    
    public string InteractName => interactName;
    public string InteractPrompt => interactPrompt;
    [Header("Lights Controlled")]
    public Light[] lights;

    [Header("Switch State")]
    public bool startOn = true;

    private bool isOn;

    void Start()
    {
        isOn = startOn;
        SetLights(isOn);
    }

    public void Interact()
    {
        ToggleLights();
    }

    void ToggleLights()
    {
        isOn = !isOn;
        SetLights(isOn);
    }

    void SetLights(bool state)
    {
        foreach (Light l in lights)
        {
            if (l != null)
            {
                l.enabled = state;
            }
        }
    }
}
