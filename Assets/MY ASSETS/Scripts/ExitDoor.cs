using UnityEngine;
using UnityEngine.Events;

public class ExitDoorTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public Notebook playerNotebook;
    public DoorLock exitDoor;

    [Header("Condición de Victoria")]
    public int requiredClues = 7;
    public UnityEvent onLevelCompleted;

    private bool levelFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (levelFinished) return;

        if (other.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>() != null || other.CompareTag("Player"))
        {
            if (playerNotebook != null && playerNotebook.GetCollectedCluesCount() >= requiredClues)
            {
                levelFinished = true;

                if (exitDoor != null)
                {
                    exitDoor.UnlockDoor();
                }

                onLevelCompleted?.Invoke();
            }
            else
            {
                int actuales = playerNotebook != null ? playerNotebook.GetCollectedCluesCount() : 0;
            }
        }
    }
}
