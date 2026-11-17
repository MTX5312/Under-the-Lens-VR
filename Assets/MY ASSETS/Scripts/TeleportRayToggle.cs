using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportRayToggle : MonoBehaviour
{
    public InputActionReference teleportModeAction;
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
        if (teleportModeAction != null)
        {
            teleportModeAction.action.Enable();
            teleportModeAction.action.performed += OnTeleportStart;
            teleportModeAction.action.canceled += OnTeleportEnd;
        }
    }

    void OnDisable()
    {
        if (teleportModeAction != null)
        {
            teleportModeAction.action.performed -= OnTeleportStart;
            teleportModeAction.action.canceled -= OnTeleportEnd;
        }
    }

    private void OnTeleportStart(InputAction.CallbackContext context)
    {
        SetRayActive(true);
    }

    private void OnTeleportEnd(InputAction.CallbackContext context)
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