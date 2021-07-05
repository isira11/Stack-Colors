using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{
    public Transform target;

    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void Update()
    {
        Vector3 new_pos = target.position + offset;
        transform.position = new Vector3(transform.position.x, new_pos.y, new_pos.z);
    }
}
