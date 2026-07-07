using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk_Collect : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string bucketTag = "Bucket";

    [Header("Milk Visual")]
    [SerializeField] private GameObject milkPlanePrefab;
    [SerializeField] private Transform milkPlaneAttachPoint;

    [Header("State")]
    [SerializeField] private bool hasMilk;

    public bool HasMilk => hasMilk;

    private GameObject _spawnedMilkPlane;

    private void OnTriggerEnter(Collider other)
    {
        HandleContact(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleContact(collision.gameObject);
    }

    private void HandleContact(GameObject other)
    {
        Milk milk = other.GetComponent<Milk>();
        if (milk != null)
        {
            TryCollectMilk(milk);
            return;
        }

        if (other.CompareTag(bucketTag))
        {
            TryPourMilk();
        }
    }

    private void TryCollectMilk(Milk milk)
    {
        if (hasMilk || milk == null)
        {
            return;
        }

        if (milk.TryScoop(this))
        {
            hasMilk = true;
            SpawnMilkPlane();
        }
    }

    private void SpawnMilkPlane()
    {
        if (milkPlanePrefab == null || _spawnedMilkPlane != null)
        {
            return;
        }

        Transform attachTo = milkPlaneAttachPoint != null ? milkPlaneAttachPoint : transform;
        _spawnedMilkPlane = Instantiate(milkPlanePrefab, attachTo);
        _spawnedMilkPlane.transform.localPosition = Vector3.zero;
        _spawnedMilkPlane.transform.localRotation = Quaternion.identity;

        // Disable Milk and collider components to avoid re-triggering interactions
        Milk milkComp = _spawnedMilkPlane.GetComponent<Milk>();
        if (milkComp != null)
        {
            milkComp.enabled = false;
        }

        Collider col = _spawnedMilkPlane.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void TryPourMilk()
    {
        if (!hasMilk)
        {
            return;
        }

        hasMilk = false;
        DestroyMilkPlane();
    }

    private void DestroyMilkPlane()
    {
        if (_spawnedMilkPlane != null)
        {
            Destroy(_spawnedMilkPlane);
            _spawnedMilkPlane = null;
        }
    }

    public void ResetMilkState()
    {
        hasMilk = false;
        DestroyMilkPlane();
    }
}
