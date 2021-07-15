using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool disable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.tag == "Player" && !disable)
        {
            disable = true;
            CoinCollector.instance.AddCoin();
            Destroy(gameObject);
        }

    }

}
