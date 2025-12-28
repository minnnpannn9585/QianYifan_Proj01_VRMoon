using System;
using UnityEngine;

public class WalkOnSphere : MonoBehaviour
{
    [Header("Sphere")]
    [SerializeField] private Transform sphereCenter;
    [SerializeField] private float sphereRadius = 10f;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSmooth = 10f;
    [SerializeField] private bool useUnityInputFallback = true; // if true, reads Unity Horizontal/Vertical as fallback

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform headTransform; // typically the VR camera

    private Vector2 moveInput = Vector2.zero;

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
        headTransform = Camera.main ? Camera.main.transform : null;
    }

    private void Start()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (headTransform == null && Camera.main != null) headTransform = Camera.main.transform;

        if (sphereCenter == null) Debug.LogError("WalkOnSphere: sphereCenter not set.");
    }

    private void Update()
    {
        if (useUnityInputFallback && moveInput == Vector2.zero)
        {
            // fallback for testing in editor
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        DoMove();
        // reset fallback input each frame (external callers should call SetInput each frame)
        if (useUnityInputFallback) moveInput = Vector2.zero;
    }

    private void DoMove()
    {
        if (sphereCenter == null) return;

        // compute local up (from sphere center to player)
        Vector3 up = (transform.position - sphereCenter.position).normalized;

        // determine forward/right relative to head, projected on tangent plane
        Vector3 forwardRef = headTransform ? headTransform.forward : transform.forward;
        Vector3 rightRef = headTransform ? headTransform.right : transform.right;

        Vector3 forwardOnTangent = Vector3.ProjectOnPlane(forwardRef, up).normalized;
        Vector3 rightOnTangent = Vector3.ProjectOnPlane(rightRef, up).normalized;

        // desired movement on tangent plane
        Vector3 desiredDirection = forwardOnTangent * moveInput.y + rightOnTangent * moveInput.x;
        if (desiredDirection.sqrMagnitude > 1f) desiredDirection.Normalize();

        Vector3 worldMove = desiredDirection * speed * Time.deltaTime;

        // tentative new position then project onto sphere surface with same radius
        Vector3 tentativePos = transform.position + worldMove;
        Vector3 radial = (tentativePos - sphereCenter.position).normalized * sphereRadius;
        Vector3 surfacePos = sphereCenter.position + radial;

        // gravity / correction vector to place the character on surface
        Vector3 gravityCorrection = surfacePos - tentativePos;

        // total move applied through CharacterController (preserves physics/collisions)
        Vector3 totalMove = worldMove + gravityCorrection;

        if (characterController != null)
        {
            characterController.Move(totalMove);
        }
        else
        {
            transform.position += totalMove;
        }

        // rotate player so up matches sphere normal and forward aligns with the tangent forward
        Vector3 targetForward = desiredDirection.sqrMagnitude > 0.001f ? Vector3.ProjectOnPlane(desiredDirection, up).normalized : Vector3.ProjectOnPlane(transform.forward, up).normalized;
        if (targetForward.sqrMagnitude < 0.001f) targetForward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        Quaternion targetRot = Quaternion.LookRotation(targetForward, up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
    }

    // Call this every frame from your XRTK input handler with the 2D axis (x = strafe, y = forward)
    public void SetInput(Vector2 input)
    {
        moveInput = input;
    }

    // Helper: call to position player exactly on sphere surface (useful when initializing)
    public void SnapToSurface()
    {
        if (sphereCenter == null) return;
        Vector3 radial = (transform.position - sphereCenter.position).normalized * sphereRadius;
        Vector3 surfacePos = sphereCenter.position + radial;
        if (characterController != null)
        {
            characterController.enabled = false;
            transform.position = surfacePos;
            characterController.enabled = true;
        }
        else
        {
            transform.position = surfacePos;
        }

        // align rotation
        Vector3 up = (transform.position - sphereCenter.position).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
