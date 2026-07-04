using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    [Tooltip("Amount to move along the Y axis each time milk is successfully scooped after props are cleared.")]
    public float decreaseAmount = 0.1f;

    [Tooltip("Maximum number of times the milk can be decreased.")]
    public int maxDecreaseCount = 3;

    [Tooltip("Props floating on this milk surface. They will be collected before the water level decreases.")]
    [SerializeField] private List<GameObject> surfaceProps = new List<GameObject>();

    private int currentCount = 0;

    public bool TryScoop(Milk_Collect collector)
    {
        if (collector == null || collector.HasMilk)
        {
            return false;
        }

        if (TryCollectSurfaceProps())
        {
            return false;
        }

        if (currentCount >= maxDecreaseCount)
        {
            return false;
        }

        Vector3 pos = transform.position;
        pos.y -= decreaseAmount;
        transform.position = pos;

        currentCount++;
        return true;
    }

    private bool TryCollectSurfaceProps()
    {
        bool collectedAny = false;

        for (int i = surfaceProps.Count - 1; i >= 0; i--)
        {
            GameObject prop = surfaceProps[i];
            if (prop == null)
            {
                surfaceProps.RemoveAt(i);
                continue;
            }

            PropId propId = prop.GetComponent<PropId>();
            if (propId != null && BookUi.Instance != null)
            {
                BookUi.Instance.UnlockPropNote(propId.propId);
            }

            Destroy(prop);
            surfaceProps.RemoveAt(i);
            collectedAny = true;
        }

        return collectedAny;
    }

    public void ResetDecreases()
    {
        currentCount = 0;
    }
}
