using UnityEngine;

public class DoorLock : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Estado de la Puerta")]
    public bool isLocked = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ApplyLockState();
    }
    
    public void UnlockDoor()
    {
        isLocked = false;
        ApplyLockState();
        Debug.Log("¡Puerta desbloqueada! Ahora se puede empujar.");
    }

    public void LockDoor()
    {
        isLocked = true;
        ApplyLockState();
        Debug.Log("La puerta ha sido trabada.");
    }

    private void ApplyLockState()
    {
        if (rb != null)
        {
            rb.isKinematic = isLocked;
        }
    }
}
