using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KickPower : MonoBehaviour
{
    public game_variables_so game_variables;

    [SerializeField] public Slider slider;


    public float force = 3;
    public bool add;
    Rigidbody rb;




    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        slider.minValue = 0;
        slider.maxValue = 20;
    }

    private void Update()
    {
        slider.value = transform.position.y;
        game_variables.kick_force = Mathf.Clamp(transform.position.y/20,0, 1);
    }

    private void FixedUpdate()
    {
        if (add)
        {
            rb.velocity = rb.velocity / 2;
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            add = false;
        }

        transform.position = new Vector3(transform.position.x,Mathf.Clamp(transform.position.y,0,20), transform.position.z);
    }

    public void Tap()
    {
        add = true;
    }
}
