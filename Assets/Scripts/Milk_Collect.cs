using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk_Collect : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string bucketTag = "Bucket";

    [Header("State")]
    [SerializeField] private bool hasMilk;
    [SerializeField] private GameObject milkVisual;

    [Header("Scoop")]
    [SerializeField] private float scoopCooldown = 1f;

    private float nextScoopTime;

    public bool HasMilk => hasMilk;

    private void Start()
    {
        RefreshMilkVisual();
    }

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
        if (other.CompareTag(bucketTag))
        {
            TryPourMilk();
            return;
        }

        if (hasMilk)
        {
            return;
        }

        Milk milk = other.GetComponent<Milk>();
        if (milk != null)
        {
            TryCollectMilk(milk);
        }
    }

    private void TryCollectMilk(Milk milk)
    {
        if (milk == null || Time.time < nextScoopTime)
        {
            return;
        }

        bool scoopedMilk = milk.TryScoop(this);
        if (scoopedMilk)
        {
            hasMilk = true;
            RefreshMilkVisual();
        }

        nextScoopTime = Time.time + scoopCooldown;
    }

    private void TryPourMilk()
    {
        if (!hasMilk)
        {
            return;
        }

        hasMilk = false;
        RefreshMilkVisual();
    }

    public void ResetMilkState()
    {
        hasMilk = false;
        RefreshMilkVisual();
    }

    private void RefreshMilkVisual()
    {
        if (milkVisual != null)
        {
            milkVisual.SetActive(hasMilk);
        }
    }
}
