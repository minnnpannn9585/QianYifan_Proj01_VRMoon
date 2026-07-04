using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk_Collect : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string bucketTag = "Bucket";

    [Header("State")]
    [SerializeField] private bool hasMilk;

    public bool HasMilk => hasMilk;

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
        }
    }

    private void TryPourMilk()
    {
        if (!hasMilk)
        {
            return;
        }

        hasMilk = false;
    }

    public void ResetMilkState()
    {
        hasMilk = false;
    }
}
