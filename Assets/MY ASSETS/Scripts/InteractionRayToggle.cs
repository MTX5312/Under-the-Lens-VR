using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class RayTriggerToggle : MonoBehaviour
{
    public InputActionReference triggerAction;
    private XRRayInteractor rayInteractor;
    private LineRenderer lineRenderer;

    void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        lineRenderer = GetComponent<LineRenderer>();

        SetRayActive(false);
    }

    void OnEnable()
    {
        if (triggerAction != null)
        {
            triggerAction.action.Enable();
            triggerAction.action.performed += OnTriggerPressed;
            triggerAction.action.canceled += OnTriggerReleased;
        }
    }

    void OnDisable()
    {
        if (triggerAction != null)
        {
            triggerAction.action.performed -= OnTriggerPressed;
            triggerAction.action.canceled -= OnTriggerReleased;
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        CancelInvoke(nameof(DisableRayDelayed));
        SetRayActive(true);
    }

    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        Invoke(nameof(DisableRayDelayed), 0.05f);
    }

    private void DisableRayDelayed()
    {
        SetRayActive(false);
    }

    private void SetRayActive(bool state)
    {
        if (rayInteractor != null) rayInteractor.enabled = state;
        if (lineRenderer != null) lineRenderer.enabled = state;
    }
}