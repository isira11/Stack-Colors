using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSlab : MonoBehaviour
{
    MeshRenderer mr;

    public int point;
    public Action<int> OnPoint;

 
    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Slab slab))
        {
            mr.material.color = new Color32(0, 0, 0, 0);

            OnPoint.Invoke(point);
        }

    }

}
