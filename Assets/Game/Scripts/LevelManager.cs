using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine;

public class LevelManager : MonoBehaviour
{
    public game_variables_so game_Variables_So;
    public GameObject player_prefab;
    public GameObject block_prefab;
    public GameObject finish_prefab;
    public GameObject point_prefab;
    public GameObject bonus_prefab;

    public Transform start_pos;
    public GameObject start_block;
    public GameObject player;
    public GameObject generated_folder;
    public Transform point_multiplier_cam_target;

    public TextMeshProUGUI collect_points_txt;
    public TextMeshProUGUI point_multiplier_txt;
    public TextMeshProUGUI points_received_txt;


    public float  collected_points 
    {
        get { return _collected_points; }
        set
        {
            if (value<0)
            {
                _collected_points = 0;
            }
            else
            {
                _collected_points = value;
            }

            collect_points_txt.SetText("" + _collected_points);
        }
    }

    float _collected_points = 0;

    Dictionary<float, float> points = new Dictionary<float, float>();
    bool point_timer_started;
    Bounds bounds;
    float highest_multipier = 0;

    public void CreateLevel()
    {
        game_Variables_So.kick_force = 0;
        point_timer_started = false;
        highest_multipier = 0;
        collected_points = 0;

        if (player)
        {
            Destroy(player);
        }

        if (generated_folder)
        {
            Destroy(generated_folder);
        }

        player = Instantiate(player_prefab, start_pos.position, Quaternion.identity);
        player.transform.Find("Player").GetComponent<Collector>().OnSlabAdded = ()=> { collected_points++; };
        player.transform.Find("Player").GetComponent<Collector>().OnSlabRemoved = () => { collected_points--; };
        player.transform.parent = transform;

        generated_folder = new GameObject("generated_folder");
        generated_folder.transform.parent = transform;

        game_Variables_So.generated_level_folder = generated_folder.transform;


        for (int i = 0; i <= 2; i++)
        {
            PlaceObject(Instantiate(block_prefab));
        }

        PlaceObject(Instantiate(finish_prefab));

        float i2 = 1.0f;
        for (int i = 0; i < 100; i++)
        {
            GameObject _ = Instantiate(point_prefab);
            _.transform.Find("ground").GetComponent<PointSlab>().point = i2;
            _.transform.Find("ground").GetComponent<PointSlab>().OnPoint = (float point , Vector3 pos)=>{OnPointScored(point, pos);};

            _.transform.Find("ground").GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            _.transform.Find("text").GetComponent<TextMeshPro>().SetText("" + i2);

            i2 += 0.2f;
            i2 = Mathf.Round(i2 * 10f) / 10f;

            if (i == 0)
            {
                point_multiplier_cam_target.position = _.transform.position;
            }
            PlaceObject(_);
        }

        PlaceObject(Instantiate(bonus_prefab));
    }


    public void OnPointScored(float point,Vector3 pos)
    {
        if (!point_timer_started)
        {
            points.Clear();
            StartCoroutine(CR_PointTimeout());
        }


        if (!points.ContainsKey(point))
        {
            if (point > highest_multipier)
            {
                highest_multipier = point;
                point_multiplier_txt.SetText("x"+highest_multipier);
            }
            points.Add(point, point);
            point_multiplier_cam_target.position = pos;
        }
    }

    IEnumerator CR_PointTimeout()
    {
        point_timer_started = true;
        player.SetActive(false);

        GameObject _cam = new GameObject("cam");
        _cam.transform.position = point_multiplier_cam_target.position;
        _cam.transform.parent = generated_folder.transform;
        _cam.AddComponent<Camera>();
        CameraController2 controller =_cam.AddComponent<CameraController2>();
        controller.target = point_multiplier_cam_target;
        controller.smooth = 3;
        controller.smooth_rot = 2;
        controller.offset = new Vector3(-11.2f, -18.1f, 22);


        while (true)
        {
            int count = points.Count;
            yield return new WaitForSeconds(2.5f);

            if (points.Count == count)
            {
                points_received_txt.SetText(""+collected_points* highest_multipier);
                GameEventMessage.SendEvent("OnPointTimeOut");
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
