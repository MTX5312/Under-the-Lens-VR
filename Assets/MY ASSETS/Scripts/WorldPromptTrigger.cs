using UnityEngine;
using UnityEngine.Events;
using Unity.XR.CoreUtils;

public class WorldPromptTrigger : MonoBehaviour
{
    [Header("UI a mostrar (World Space Canvas o ícono)")]
    public GameObject promptObject;

    [Header("Comportamiento visual")]
    public bool faceCamera = true;
    public bool hideOrDestroyOnComplete = true;
    public bool destroyWhenCompleted = false;

    [Header("Evento al completar la acción (asignable desde el Inspector)")]
    public UnityEvent onActionCompleted;

    private bool isCompleted = false;
    private Transform cameraTransform;

    void Awake()
    {
        if (promptObject != null)
            promptObject.SetActive(false);

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (isCompleted || !faceCamera || promptObject == null || !promptObject.activeSelf)
            return;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (cameraTransform != null)
        {
            promptObject.transform.LookAt(
                promptObject.transform.position + cameraTransform.rotation * Vector3.forward,
                cameraTransform.rotation * Vector3.up
            );
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player")
            || other.GetComponentInParent<XROrigin>() != null
            || other.GetComponentInParent<CharacterController>() != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;

        if (IsPlayer(other) && promptObject != null)
        {
            promptObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCompleted) return;

        if (IsPlayer(other) && promptObject != null)
        {
            promptObject.SetActive(false);
        }
    }

    public void CompleteAction()
    {
        if (isCompleted) return;
        isCompleted = true;

        onActionCompleted?.Invoke();

        if (hideOrDestroyOnComplete)
        {
            ApplyHideOrDestroy();
        }
    }

    public void HideOrDestroyPrompt()
    {
        isCompleted = true;
        ApplyHideOrDestroy();
    }

    private void ApplyHideOrDestroy()
    {
        if (promptObject != null)
        {
            if (destroyWhenCompleted)
                Destroy(promptObject);
            else
                promptObject.SetActive(false);
        }

        if (destroyWhenCompleted)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}