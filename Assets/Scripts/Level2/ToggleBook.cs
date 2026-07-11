using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleBook : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference toggleAction;

    [Header("Target")]
    [SerializeField] private GameObject bookCanvas;

    private void OnEnable()
    {
        if (toggleAction != null && toggleAction.action != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnTogglePerformed;
        }
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
        if (bookCanvas == null)
        {
            return;
        }

        bookCanvas.SetActive(!bookCanvas.activeSelf);
    }
}
