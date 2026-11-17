using UnityEngine;

public class DoorLock : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Estado de la Puerta")]
    public bool isLocked = true;

    [Header("Feedback Audiovisual")]
    public string roomName = "Principal";
    public AudioClip unlockSound;
    [Range(0f, 1f)] public float soundVolume = 1f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ApplyLockState();
    }
    
    public void UnlockDoor()
    {
        if (!isLocked) return;
        isLocked = false;
        ApplyLockState();
        Debug.Log("¡Puerta desbloqueada!");
        if (unlockSound != null)
            AudioSource.PlayClipAtPoint(unlockSound, transform.position, soundVolume);

        if (VRNotificationHUD.Instance != null)
            VRNotificationHUD.Instance.ShowNotification($"Puerta {roomName} abierta");
    }

    public void LockDoor()
    {
        isLocked = true;
        ApplyLockState();
        Debug.Log("La puerta ha sido trabada");
    }

    private void ApplyLockState()
    {
        if (rb != null)
        {
            rb.isKinematic = isLocked;
        }
    }
}
