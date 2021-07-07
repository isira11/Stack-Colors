using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slab : MonoBehaviour
{
    public bool inBag = false;

    public void StartDestroy(float delay=1,float t1=0.1f, float t2 = 0.4f)
    {
        transform.parent = null;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();

        rb.AddForce(Vector3.forward * Random.Range(0,1000));
        rb.angularVelocity = new Vector3(Random.Range(0,1000), Random.Range(0, 1000), Random.Range(0, 1000));

        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(Random.Range(0.5f, delay));
        sequence.Append(transform.DOScale(transform.localScale*1.3f, t1));
        sequence.Append(transform.DOScale(Vector3.zero, t2).SetEase(Ease.InCirc));

        sequence.OnComplete(()=> {
            Destroy(gameObject);
        });
    }

    public void StartDestroy2()
    {
        StartDestroy(0.1f, 0.1f, 0.2f);
    }

    public Color GetColor()
    {
        return GetComponent<MeshRenderer>().material.color;
    }

}
