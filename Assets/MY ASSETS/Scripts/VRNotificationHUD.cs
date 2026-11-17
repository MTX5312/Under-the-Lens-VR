using System.Collections;
using UnityEngine;
using TMPro;

public class VRNotificationHUD : MonoBehaviour
{
    public static VRNotificationHUD Instance { get; private set; }

    [Header("Referencias UI")]
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;

    [Header("Configuración")]
    public float displayDuration = 3f;

    private Coroutine hideCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (notificationPanel == null || notificationText == null) return;

        notificationText.text = message;
        notificationPanel.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        notificationPanel.SetActive(false);
    }
}
