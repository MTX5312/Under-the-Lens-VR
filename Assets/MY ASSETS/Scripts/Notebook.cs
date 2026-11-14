using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Notebook : MonoBehaviour
{
    [Header("Referencias de UI (TextMeshPro)")]
    public TextMeshProUGUI pageTitleText;
    public TextMeshProUGUI pageBodyText;

    [Header("Configuración de Páginas")]
    private List<string> notebookPages = new List<string> {
        "Nota Inicial: He llegado a la casa. Debo buscar pistas ocultas usando la lupa."
    };
    private int currentPageIndex = 0;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        grabInteractable.activated.AddListener(OnTriggerPressed);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
        grabInteractable.activated.RemoveListener(OnTriggerPressed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    private void OnTriggerPressed(ActivateEventArgs args)
    {
        if (!isHeld) return;

        NextPage();
    }

    public void NextPage()
    {
        if (notebookPages.Count == 0) return;

        currentPageIndex = (currentPageIndex + 1) % notebookPages.Count;
        UpdateNotebookUI();
    }

    public void AddClue(string clueDescription)
    {
        notebookPages.Add(clueDescription);

        currentPageIndex = notebookPages.Count - 1;
        UpdateNotebookUI();
    }

    private void UpdateNotebookUI()
    {
        if (pageBodyText != null)
        {
            pageBodyText.text = notebookPages[currentPageIndex];
        }

        if (pageTitleText != null)
        {
            pageTitleText.text = $"Página {currentPageIndex + 1} de {notebookPages.Count}";
        }
    }
}