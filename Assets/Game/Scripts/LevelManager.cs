using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject player_prefab;
    public GameObject block_prefab;

    public Transform start_pos;
    public GameObject start_block;
    public GameObject player;

    public GameObject generated_folder;


    Bounds bounds;


    public void CreateLevel()
    {
        if (player)
        {
            Destroy(player);
        }

        if (generated_folder)
        {
            Destroy(generated_folder);
        }

        player = Instantiate(player_prefab, start_pos.position, Quaternion.identity);
        player.transform.parent = transform;

        generated_folder = new GameObject("generated_folder");
        generated_folder.transform.parent = transform;


        for (int i = 0; i < 1; i++)
        {
            GameObject gameObject = Instantiate(block_prefab);
            gameObject.transform.parent = generated_folder.transform;
            gameObject.transform.position = Vector3.forward * bounds.max.z;
            gameObject.transform.position += Vector3.forward * gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.z/2;
            bounds = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().bounds;
        }
    }

    public void OnMenu()
    {
        bounds = start_block.transform.GetChild(0).GetComponent<MeshRenderer>().bounds;
        CreateLevel();
    }
}
