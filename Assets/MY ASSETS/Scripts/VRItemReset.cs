using System.Collections;
using UnityEngine;

public class VRItemReset : MonoBehaviour
{
    [Header("Posición local deseada frente a la cámara")]
    public Vector3 localPositionOffset = new Vector3(0f, 0f, 0f);
    public Vector3 localRotationOffset = new Vector3(0f, 0f, 0f);

    void Start()
    {
        StartCoroutine(ResetPositionRoutine());
    }

    private IEnumerator ResetPositionRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        transform.localPosition = localPositionOffset;
        transform.localEulerAngles = localRotationOffset;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    public void OnGrabbed()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}