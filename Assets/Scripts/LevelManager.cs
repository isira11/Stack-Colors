using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public GameObject block;

    Bounds bounds;

    private void Start()
    {
        bounds = new Bounds();
        CreateLevel();
    }

    public void CreateLevel()
    {



        for (int i = 0; i < 10; i++)
        {

            GameObject gameObject = Instantiate(block);

            gameObject.transform.position = Vector3.forward * bounds.max.z;
            gameObject.transform.position += Vector3.forward * gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.z/2;
            bounds = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().bounds;

            //gameObject.transform.position += Vector3.forward * bounds.size.z;


        }



    }
}
