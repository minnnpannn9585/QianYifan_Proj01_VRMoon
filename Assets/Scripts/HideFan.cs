using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFan : MonoBehaviour
{
    public GameObject hand1;
    public GameObject hand2;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fan")
        {
            hand1.SetActive(true);
            hand2.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }
}
