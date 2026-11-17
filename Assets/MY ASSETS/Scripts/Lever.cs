using UnityEngine;
using UnityEngine.Events;

public class VRLever : MonoBehaviour
{
    public Animator leverAnimator;
    public string animatorParameter = "LeverUp";
    public bool useTriggerInsteadOfBool = false;
    public DoorLock targetDoor;
    public UnityEvent onLeverPulled;

    private bool isActivated = false;

    void Awake()
    {
        if (leverAnimator == null)
            leverAnimator = GetComponent<Animator>();
    }

    public void ToggleLever()
    {
        if (isActivated) return;
        isActivated = true;

        if (leverAnimator != null)
        {
            if (useTriggerInsteadOfBool)
                leverAnimator.SetTrigger(animatorParameter);
            else
                leverAnimator.SetBool(animatorParameter, true);
        }

        if (targetDoor != null)
        {
            targetDoor.UnlockDoor();
        }

        onLeverPulled?.Invoke();
    }
}