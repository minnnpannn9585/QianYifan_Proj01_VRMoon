using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateRightRay : MonoBehaviour
{
    [SerializeField] private GameObject rayVisual;
    [SerializeField] private RectTransform targetPanel;
    [SerializeField] private float maxDistance = 10f;
    public GameObject handObject;

    private void Reset()
    {
        rayVisual = gameObject;
    }

    private void Update()
    {
        if (rayVisual == null)
        {
            return;
        }

        rayVisual.SetActive(IsHoveringTargetPanel());
        handObject.SetActive(!IsHoveringTargetPanel());
    }

    private bool IsHoveringTargetPanel()
    {
        if (targetPanel == null)
        {
            return false;
        }

        if (!targetPanel.gameObject.activeInHierarchy)
        {
            return false;
        }

        CanvasGroup canvasGroup = targetPanel.GetComponentInParent<CanvasGroup>();
        if (canvasGroup != null && (canvasGroup.alpha <= 0f || !canvasGroup.interactable))
        {
            return false;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        Plane panelPlane = new Plane(targetPanel.forward, targetPanel.position);

        if (!panelPlane.Raycast(ray, out float enter))
        {
            return false;
        }

        if (enter < 0f || enter > maxDistance)
        {
            return false;
        }

        Vector3 hitPoint = ray.GetPoint(enter);
        Vector2 localPoint = targetPanel.InverseTransformPoint(hitPoint);
        Rect rect = targetPanel.rect;

        return rect.Contains(localPoint);
    }
}
