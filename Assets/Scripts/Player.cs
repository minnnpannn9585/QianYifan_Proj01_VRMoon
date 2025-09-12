using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        // Debug.Log(h);
        float v = Input.GetAxis("Vertical");
        rd.AddForce(new Vector3(h, 0, v));
    }
}
