using System.Collections;
using UnityEngine;

public class FishCollectEffect : MonoBehaviour
{
    [Header("Effect")]
    [SerializeField] private ParticleSystem dissolveEffectPrefab;
    [SerializeField] private float effectDestroyDelay = 5f;

    [Header("Mesh")]
    [SerializeField] private Transform meshRoot;
    [SerializeField] private float scaleDownDuration = 0.5f;

    private bool isCollected;
    private Collider cachedCollider;
    private RandomFloatMovement randomFloatMovement;
    private Vector3 originalScale;

    private void Awake()
    {
        cachedCollider = GetComponent<Collider>();
        randomFloatMovement = GetComponent<RandomFloatMovement>();

        if (meshRoot == null && transform.childCount > 0)
        {
            meshRoot = transform.GetChild(0);
        }

        if (meshRoot != null)
        {
            originalScale = meshRoot.localScale;
        }
    }

    public void Collect()
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;

        if (cachedCollider != null)
        {
            cachedCollider.enabled = false;
        }

        if (randomFloatMovement != null)
        {
            randomFloatMovement.enabled = false;
        }

        if (dissolveEffectPrefab != null)
        {
            Vector3 effectPosition = meshRoot != null ? meshRoot.position : transform.position;
            ParticleSystem effectInstance = Instantiate(
                dissolveEffectPrefab,
                effectPosition,
                Quaternion.identity
            );

            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectDestroyDelay);
        }

        StartCoroutine(ScaleDownAndDestroy());
    }

    private IEnumerator ScaleDownAndDestroy()
    {
        if (meshRoot == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < scaleDownDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / scaleDownDuration);
            meshRoot.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        meshRoot.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}