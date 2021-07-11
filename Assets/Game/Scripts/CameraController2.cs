using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smooth;
    public float smooth_rot = 5;

    private void Start()
    {

    }
    private void LateUpdate()
    {
        Vector3 new_pos = -target.InverseTransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position,new_pos,Time.deltaTime* smooth);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position-transform.position),Time.deltaTime* smooth_rot);

    }

}
