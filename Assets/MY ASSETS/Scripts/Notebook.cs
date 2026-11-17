using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Notebook : MonoBehaviour
{
    [System.Serializable]
    public struct NotebookPage
    {
        public string title;
        [TextArea(3, 10)]
        public string bodyText;
        public Sprite clueImage;
    }

    [Header("Referencias de Interfaz (UI)")]
    public TextMeshProUGUI pageTitleText;
    public TextMeshProUGUI pageBodyText;
    public Image clueImageUI;

    [Header("Animación")]
    public Animator notebookAnimator;

    [Header("Configuración de Páginas")]
    [SerializeField]
    private List<NotebookPage> notebookPages = new List<NotebookPage> {
        new NotebookPage {
            title = "",
            bodyText = "",
            clueImage = null
        }
    };

    private int currentPageIndex = 0;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Start()
    {
        UpdateNotebookUI();
    }

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args) => isHeld = true;

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;

        if (currentPageIndex != 0)
        {
            currentPageIndex = 0;

            if (notebookAnimator != null)
            {
                notebookAnimator.ResetTrigger("TurnPage");
                notebookAnimator.ResetTrigger("ResetPage");

                notebookAnimator.SetTrigger("ResetPage");
            }

            UpdateNotebookUI();
        }
    }

    void Update()
    {
        if (isHeld && Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            NextPage();
        }
    }

    public void NextPage()
    {
        if (notebookPages.Count <= 1) return;

        currentPageIndex = (currentPageIndex + 1) % notebookPages.Count;

        if (notebookAnimator != null)
        {
            notebookAnimator.ResetTrigger("TurnPage");
            notebookAnimator.ResetTrigger("ResetPage");

            if (currentPageIndex == 0)
            {
                notebookAnimator.SetTrigger("ResetPage");
            }
            else
            {
                notebookAnimator.SetTrigger("TurnPage");
            }
        }

        UpdateNotebookUI();
    }

    public void AddNewClue(string title, string description, Sprite image)
    {
        NotebookPage newPage = new NotebookPage
        {
            title = title,
            bodyText = description,
            clueImage = image
        };

        notebookPages.Add(newPage);
    }

    private void UpdateNotebookUI()
    {
        if (notebookPages.Count == 0) return;

        NotebookPage current = notebookPages[currentPageIndex];

        if (pageTitleText != null) pageTitleText.text = current.title;
        if (pageBodyText != null) pageBodyText.text = current.bodyText;

        if (clueImageUI != null)
        {
            if (current.clueImage != null)
            {
                clueImageUI.gameObject.SetActive(true);
                clueImageUI.sprite = current.clueImage;
            }
            else
            {
                clueImageUI.gameObject.SetActive(false);
            }
        }
    }

    public int GetCollectedCluesCount()
    {
        return notebookPages.Count;
    }
}