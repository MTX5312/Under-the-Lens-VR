using UnityEngine;
using UnityEngine.Events;

public class VRLever : MonoBehaviour
{
    [Header("Puerta")]
    public DoorLock targetDoor;

    public UnityEvent onLeverActivated;

    private bool isActivated = false;

    public void ActivateLever()
    {
        if (isActivated) return;
        isActivated = true;

        if (targetDoor != null)
        {
            targetDoor.UnlockDoor();
        }

        onLeverActivated?.Invoke();
    }
}
