using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Milk : MonoBehaviour
{
    [Tooltip("Amount to move along the Z axis each time a ladle hits the milk.")]
    public float decreaseAmount = 0.1f;

    [Tooltip("Maximum number of times the milk can be decreased.")]
    public int maxDecreaseCount = 3;

    // tracks how many times we've decreased so far
    private int currentCount = 0;

    // Support both trigger and non-trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladle"))
        {
            HandleHit();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ladle"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        if (currentCount >= maxDecreaseCount)
            return;

        Vector3 pos = transform.position;
        pos.y -= decreaseAmount;
        transform.position = pos;

        currentCount++;
    }

    // Optional: expose a way to reset the counter from other scripts or the inspector
    public void ResetDecreases()
    {
        currentCount = 0;
    }
}
