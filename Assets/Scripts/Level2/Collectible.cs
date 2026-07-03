using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject objectNote;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hand")
        {
            objectNote.SetActive(true);
        }
    }
}
