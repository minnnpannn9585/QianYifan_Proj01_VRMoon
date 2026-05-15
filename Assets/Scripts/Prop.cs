using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int count;
    public BoatRoute boat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            Destroy(other.gameObject);
            count++;
            if (count == 5)
            {
                boat.canMove = true;
                count = 0;
            }
        }
    }
}
