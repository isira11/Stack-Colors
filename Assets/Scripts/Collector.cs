using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collector : MonoBehaviour
{
    public Transform folk;
    public LinkedList<Slab> slabs = new LinkedList<Slab>();
    public Queue<Slab> buffer = new Queue<Slab>();
    public bool lifting;

    private void Start()
    {
        DOTween.Init();
    }

    private void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Slab slab))
        {
            if (slab.inBag)
            {
                return;
            }
            slab.inBag = true;
            collision.transform.parent = folk.parent;
            collision.transform.rotation = folk.parent.rotation;
            collision.transform.DOLocalMove(Vector3.zero + folk.parent.up * (collision.transform.localScale.y / 2 + 0.2271941f / 2), 0.1f)
                .SetEase(Ease.InOutCirc)
                .OnComplete(() =>
                {
                    buffer.Enqueue(slab);

                    if (!lifting)
                    {
                        OnAddSlab(buffer.Dequeue());
                    }
                });
        }

        if(collision.transform.tag == "enemy")
        {
            if (collision.contacts[0].thisCollider.transform.TryGetComponent(out Slab collider_slab))
            {
                LinkedListNode<Slab> linkedListNode = slabs.Find(collider_slab);

                while (linkedListNode != null)
                {
                    Slab _slab = linkedListNode.Value;
    
                    linkedListNode = linkedListNode.Next;
                    slabs.Remove(_slab);
                    _slab.StartDestroy();
                }
            }
        }
    }


    public void OnAddSlab(Slab slab)
    {
        lifting = true;

        Sequence sequence = DOTween.Sequence();


        sequence.Append(folk.transform.DOLocalMoveY(folk.transform.localPosition.y + slab.transform.localScale.y + 0.2f,0.01f));
        sequence.Append(folk.transform.DOLocalMoveY(folk.transform.localPosition.y + slab.transform.localScale.y + 0.1f, 0.05f));

        sequence.OnComplete(() =>
                {
                    lifting = false;
                    slab.transform.parent = folk;
                    slabs.AddFirst(slab);
                    print(slabs.Count);
                    if (buffer.Count > 0)
                    {
                        Slab _ = buffer.Dequeue();
                        OnAddSlab(_);

                   
                    }

                });

    }
}
