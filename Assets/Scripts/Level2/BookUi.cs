using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookUi : MonoBehaviour
{
    [Serializable]
    private class PropNoteEntry
    {
        public int propId;
        public GameObject noteObject;
    }

    [Header("Input")]
    [SerializeField] private InputActionReference toggleAction;

    [Header("Target")]
    [SerializeField] private GameObject target;

    [Header("Pages")]
    [SerializeField] private GameObject[] pages;
    [SerializeField] private int currentPageIndex = 0;

    [Header("Prop Notes")]
    [SerializeField] private PropNoteEntry[] propNotes;

    public static BookUi Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Reset()
    {
        target = gameObject;
    }

    private void OnEnable()
    {
        if (toggleAction != null && toggleAction.action != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnTogglePerformed;
        }

        RefreshPages();
    }

    private void OnDisable()
    {
        if (toggleAction != null && toggleAction.action != null)
        {
            toggleAction.action.performed -= OnTogglePerformed;
            toggleAction.action.Disable();
        }
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        if (target == null)
        {
            return;
        }

        bool nextActive = !target.activeSelf;
        target.SetActive(nextActive);

        if (nextActive)
        {
            RefreshPages();
        }
    }

    public void ShowPreviousPage()
    {
        if (pages == null || pages.Length == 0)
        {
            return;
        }

        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = 0;
        }

        RefreshPages();
    }

    public void ShowNextPage()
    {
        if (pages == null || pages.Length == 0)
        {
            return;
        }

        currentPageIndex++;
        if (currentPageIndex >= pages.Length)
        {
            currentPageIndex = pages.Length - 1;
        }

        RefreshPages();
    }

    public void UnlockPropNote(int propId)
    {
        if (propNotes == null)
        {
            return;
        }

        for (int i = 0; i < propNotes.Length; i++)
        {
            if (propNotes[i] != null && propNotes[i].propId == propId && propNotes[i].noteObject != null)
            {
                propNotes[i].noteObject.SetActive(true);
            }
        }
    }

    private void RefreshPages()
    {
        if (pages == null || pages.Length == 0)
        {
            return;
        }

        if (currentPageIndex < 0)
        {
            currentPageIndex = 0;
        }

        if (currentPageIndex >= pages.Length)
        {
            currentPageIndex = pages.Length - 1;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
            {
                pages[i].SetActive(i == currentPageIndex);
            }
        }
    }
}