using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slab : MonoBehaviour
{
    public bool inBag = false;

    public void StartDestroy()
    {
        transform.parent = null;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();

        rb.AddForce(Vector3.forward * Random.Range(0,1000));
        rb.angularVelocity = new Vector3(Random.Range(0,1000), Random.Range(0, 1000), Random.Range(0, 1000));

        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(Random.Range(0.5f, 1));
        sequence.Append(transform.DOScale(transform.localScale*1.3f, 0.1f));
        sequence.Append(transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InCirc));

        sequence.OnComplete(()=> {
            Destroy(gameObject);
        });
    }

}
