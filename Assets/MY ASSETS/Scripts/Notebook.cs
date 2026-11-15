using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NotebookManager : MonoBehaviour
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

    [Header("Animacion")]
    public Animator notebookAnimator;

    [Header("Configuracion de Paginas")]
    [SerializeField]
    private List<NotebookPage> notebookPages = new List<NotebookPage> {
        new NotebookPage {
            title = "",
            bodyText = "",
            clueImage = null
        }
    };

    private int currentPageIndex = 0;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    [Header("Input del Trigger")]
    public InputActionReference triggerAction;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        if (triggerAction != null && triggerAction.action != null)
        {
            triggerAction.action.performed += OnTriggerPerformed;
            triggerAction.action.Enable();
        }
    }
    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);

        if (triggerAction != null && triggerAction.action != null)
        {
            triggerAction.action.performed -= OnTriggerPerformed;
            triggerAction.action.Disable();
        }
    }
    private void OnGrabbed(SelectEnterEventArgs args) => isHeld = true;
    private void OnReleased(SelectExitEventArgs args) => isHeld = false;
    void Update()
    {
        if (isHeld && Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            NextPage();
        }
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            NextPage();
        }
    }

    public void NextPage()
    {
        if (notebookPages.Count <= 1) return;

        int previousIndex = currentPageIndex;
        currentPageIndex = (currentPageIndex + 1) % notebookPages.Count;

        if (notebookAnimator != null)
        {
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
        currentPageIndex = notebookPages.Count - 1;
        UpdateNotebookUI();
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

    void Start()
    {
        UpdateNotebookUI();
    }
}