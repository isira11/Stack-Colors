using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Doozy.Engine;
public class Collector : MonoBehaviour
{
    public InputController inputController;
    public MeshRenderer fork_model;
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

            if (slab.GetColor() == fork_model.material.color)
            {
                //print("SAME");
            }
            else
            {
                Destroy(slab);
                RemoveSlabFromBottom();
                return;
            }

            slab.inBag = true;
            collision.transform.parent = folk.parent;
            collision.transform.rotation = folk.parent.rotation;
            collision.transform.DOLocalMove(Vector3.zero + folk.parent.up * (collision.transform.localScale.y / 2 + fork_model.transform.localScale.y/2), 0.1f)
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

        if(collision.transform.tag == "color_wall")
        {
            Color wall_color = collision.transform.GetComponent<MeshRenderer>().material.color;
            fork_model.material.color = new Color(wall_color.r, wall_color.g, wall_color.b, 1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "finish_line_0")
        {
            KickAll();
        }

        if (other.tag == "finish_line_1")
        {

        }
    }

    public void RemoveSlabFromBottom()
    {
        if (slabs.Count>0)
        {
            slabs.Last.Value.StartDestroy2();
            slabs.RemoveLast();

        }
        else
        {
            GameEventMessage.SendEvent("game_over");
        }
    }

    public void OnAddSlab(Slab slab)
    {
        if (lifting && slab ) return;
        lifting = true;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(folk.transform.DOLocalMoveY(folk.transform.localPosition.y + slab.transform.localScale.y + 0.2f, 0.01f));
        sequence.Append(folk.transform.DOLocalMoveY(folk.transform.localPosition.y + slab.transform.localScale.y + 0.1f, 0.01f));

        sequence.OnComplete(() =>
                {
                    lifting = false;
                    slab.transform.parent = folk;
                    slabs.AddFirst(slab);
                    if (buffer.Count > 0)
                    {
                        Slab _ = buffer.Dequeue();
                        OnAddSlab(_);

                   
                    }

                });

    }




    public void KickAll()
    {
        inputController.OnGameOver();
        foreach (Slab item in slabs)
        {

            item.transform.parent = null;
            Rigidbody rb = item.gameObject.AddComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * 1000);
            Destroy(item);
        }    
    }

}
