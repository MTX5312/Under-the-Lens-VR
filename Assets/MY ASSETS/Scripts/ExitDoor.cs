using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitDoorTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public Notebook playerNotebook;
    public DoorLock exitDoor;

    [Header("Condición de Victoria")]
    public int requiredClues = 7;
    public UnityEvent onLevelCompleted;

    [Header("Retorno al Menú Principal")]
    public bool loadMenuOnComplete = true;

#if UNITY_EDITOR
    [Tooltip("Arrastra aquí tu escena MainMenu desde la carpeta Scenes")]
    public SceneAsset mainMenuSceneAsset;
#endif

    [HideInInspector]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public float returnDelay = 1.5f;

    private bool levelFinished = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (mainMenuSceneAsset != null)
        {
            mainMenuSceneName = mainMenuSceneAsset.name;
        }
    }
#endif

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

                if (loadMenuOnComplete && !string.IsNullOrEmpty(mainMenuSceneName))
                {
                    StartCoroutine(ReturnToMainMenuRoutine());
                }
            }
            else
            {
                int actuales = playerNotebook != null ? playerNotebook.GetCollectedCluesCount() : 0;
            }
        }
    }

    private IEnumerator ReturnToMainMenuRoutine()
    {
        if (returnDelay > 0f)
            yield return new WaitForSeconds(returnDelay);

        SceneManager.LoadScene(mainMenuSceneName);
    }
}