using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Clue : MonoBehaviour
{
    [Header("Informacion de la Pista")]
    [Tooltip("El titulo de la pagina")]
    public string clueTitle = "";

    [TextArea(3, 10)]
    [Tooltip("Descripcion de la pagina")]
    public string clueDescription = "";

    [Tooltip("Imagen de la pagina")]
    public Sprite clueSprite;

    [Header("Recogido")]
    public bool destroyOnCollect = true;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
    }

    void OnEnable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnClueCollected);
        }
    }

    void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnClueCollected);
        }
    }

    private void OnClueCollected(SelectEnterEventArgs args)
    {
        NotebookManager notebook = FindAnyObjectByType<NotebookManager>();

        if (notebook != null)
        {
            notebook.AddNewClue(clueTitle, clueDescription, clueSprite);
            Debug.Log($"Pista registrada: {clueTitle}");
        }
        else
        {
            Debug.LogWarning("No se encontro ningun Notebook.");
        }

        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
    }
}