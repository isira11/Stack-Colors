using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelManager : MonoBehaviour
{
    public GameObject player_prefab;
    public GameObject block_prefab;
    public GameObject finish_prefab;
    public GameObject point_prefab;
    public GameObject bonus_prefab;

    public Transform start_pos;
    public GameObject start_block;
    public GameObject player;
    public GameObject generated_folder;

    Dictionary<int, int> points = new Dictionary<int, int>();
    bool point_timer_started;
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

        for (int i = 0; i <= 2; i++)
        {
            PlaceObject(Instantiate(block_prefab));
        }

        PlaceObject(Instantiate(finish_prefab));

        for (int i = 0; i < 100; i++)
        {
            GameObject _ = Instantiate(point_prefab);
            _.transform.Find("ground").GetComponent<PointSlab>().point = i + 1;
            _.transform.Find("ground").GetComponent<PointSlab>().OnPoint = (int i)=>{ OnPointScored(i); };

            _.transform.Find("ground").GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            _.transform.Find("text").GetComponent<TextMeshPro>().SetText("" + (i + 1));

            PlaceObject(_);
        }

        PlaceObject(Instantiate(bonus_prefab));
    }


    public void OnPointScored(int point)
    {
        if (!points.ContainsKey(point))
        {
            points.Add(point, point);
        }

        if (!point_timer_started)
        {
            point_timer_started = true;
            StartCoroutine(CR_PointTimeout());
        }


    }

    IEnumerator CR_PointTimeout()
    {
        while (true)
        {
            int count = points.Count;
            yield return new WaitForSeconds(2.5f);

            if (points.Count == count)
            {

                foreach (var item in points)
                {

                }
                break;
            }
        }
    }

    public void PlaceObject(GameObject obj)
    {
        obj.transform.parent = generated_folder.transform;
        obj.transform.position = Vector3.forward * bounds.max.z;
        obj.transform.position += Vector3.forward * obj.transform.Find("ground").GetComponent<MeshRenderer>().bounds.size.z / 2;
        bounds = obj.transform.Find("ground").GetComponent<MeshRenderer>().bounds;
    }

    public void OnMenu()
    {
        bounds = start_block.transform.GetChild(0).GetComponent<MeshRenderer>().bounds;
        CreateLevel();
    }
}
