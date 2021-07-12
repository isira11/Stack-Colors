using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce_effect : MonoBehaviour
{
    public float force = 3;
    public bool add;
    Rigidbody rb;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (add)
        {
            rb.velocity = rb.velocity / 2;
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            add = false;
        }

    }

}
