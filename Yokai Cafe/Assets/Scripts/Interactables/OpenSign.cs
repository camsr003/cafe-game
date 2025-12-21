
using UnityEngine;
public class OpenSign : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (DayManager.Instance.CurrentState == DayState.Preparing)
        {
            DayManager.Instance.SetState(DayState.Serving);
        }
        else if (DayManager.Instance.CurrentState == DayState.Serving)
        {
            if (DayManager.Instance.HasCustomers())
            {
                Debug.Log("Can't close yet — customers still inside!");
                return;
            }
            
            DayManager.Instance.SetState(DayState.Closing);
        }
    }
}
