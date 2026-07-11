using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    [Tooltip("Maximum number of times the milk can be scooped.")]
    public int maxDecreaseCount = 3;

    [Tooltip("World-space Y distance the milk moves down after each successful scoop.")]
    [SerializeField] private float decreaseAmount = 0.15f;

    [Tooltip("Props floating on this milk surface. One random prop is collected before milk can be scooped.")]
    [SerializeField] private List<GameObject> surfaceProps = new List<GameObject>();

    [Header("Effect")]
    [SerializeField] private GameObject scoopEffectObject;
    [SerializeField] private float scoopEffectDuration = 1f;

    private int currentCount = 0;
    private Coroutine scoopEffectCoroutine;

    public bool TryScoop(Milk_Collect collector)
    {
        if (collector == null || collector.HasMilk)
        {
            return false;
        }

        CleanupNullSurfaceProps();

        if (surfaceProps.Count > 0)
        {
            CollectRandomSurfaceProp();
            PlayScoopEffect();
            return false;
        }

        if (currentCount >= maxDecreaseCount)
        {
            return false;
        }

        currentCount++;

        Vector3 position = transform.position;
        position.y -= decreaseAmount;
        transform.position = position;

        PlayScoopEffect();

        if (currentCount >= maxDecreaseCount)
        {
            Destroy(gameObject);
        }

        return true;
    }

    public bool HasSurfaceProps()
    {
        CleanupNullSurfaceProps();
        return surfaceProps.Count > 0;
    }

    private void CollectRandomSurfaceProp()
    {
        if (surfaceProps.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, surfaceProps.Count);
        GameObject prop = surfaceProps[randomIndex];
        surfaceProps.RemoveAt(randomIndex);

        if (prop == null)
        {
            return;
        }

        PropId propId = prop.GetComponent<PropId>();
        if (propId != null && BookUi.Instance != null)
        {
            BookUi.Instance.UnlockPropNote(propId.propId);
        }

        Destroy(prop);
    }

    private void CleanupNullSurfaceProps()
    {
        for (int i = surfaceProps.Count - 1; i >= 0; i--)
        {
            if (surfaceProps[i] == null)
            {
                surfaceProps.RemoveAt(i);
            }
        }
    }

    private void PlayScoopEffect()
    {
        if (scoopEffectObject == null)
        {
            return;
        }

        if (scoopEffectCoroutine != null)
        {
            StopCoroutine(scoopEffectCoroutine);
        }

        scoopEffectCoroutine = StartCoroutine(PlayScoopEffectRoutine());
    }

    private IEnumerator PlayScoopEffectRoutine()
    {
        scoopEffectObject.SetActive(false);
        yield return null;
        scoopEffectObject.SetActive(true);
        yield return new WaitForSeconds(scoopEffectDuration);
        scoopEffectObject.SetActive(false);
        scoopEffectCoroutine = null;
    }

    public void ResetDecreases()
    {
        currentCount = 0;
    }
}
