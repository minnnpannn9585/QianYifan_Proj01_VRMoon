using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


[RequireComponent(typeof(WalkOnSphere))]
public class XRTKInputAdapter : MonoBehaviour
{
    [Tooltip("Assign a Vector2 InputAction (usually named Move / Primary2DAxis) from your Input Actions asset or XRTK binding.")]
    public InputActionReference moveAction;

    private WalkOnSphere walker;

    private void Awake()
    {
        walker = GetComponent<WalkOnSphere>();
    }

    private void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.performed += OnMovePerformed;
            moveAction.action.canceled += OnMoveCanceled;
            moveAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.performed -= OnMovePerformed;
            moveAction.action.canceled -= OnMoveCanceled;
            moveAction.action.Disable();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        walker.SetInput(v);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        walker.SetInput(Vector2.zero);
    }
}
