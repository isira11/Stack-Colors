using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public game_variables_so game_variables_so;
    public Camera cam;
    public float x_boundry_min = -7;
    public float x_boundry_max = 7;
    public float forward_speed = 2;
    public float smooth = 5;
    public Transform fork;
    public Action OnPassLine1;

    Vector3 intersect_0;
    Vector3 pos_0;
    Vector3 next_pos;
    Vector2 mouse_pos_0;

    Bounds bounds;
    bool force_move;

    bool play;

    private void Start()
    {
        game_variables_so.kick_force = 0.0f;
        next_pos = transform.position;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (!play)
        {
            return;
        }
        if (Input.touchCount > 0 && !force_move)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    intersect_0 = GetIntersectPoint();
                    pos_0 = transform.position;
                    mouse_pos_0 = Input.GetTouch(0).position;
                    break;
                case TouchPhase.Moved:
                    if (mouse_pos_0 - Input.GetTouch(0).position != Vector2.zero)
                    {

                        Vector3 delta = GetIntersectPoint() - intersect_0;
                        Vector3 new_pos = pos_0 + delta;

                        bounds = fork.GetComponent<MeshRenderer>().bounds;

                        float size1 = Mathf.Abs(bounds.min.x - transform.position.x);
                        float size2 = Mathf.Abs(bounds.max.x - transform.position.x);

                        float clamped_x = Mathf.Clamp(new_pos.x, x_boundry_min + size1, x_boundry_max - size2);
                        new_pos = new Vector3(clamped_x, new_pos.y, new_pos.z);

                        if (transform.position != new_pos)
                        {
                            next_pos = new_pos;
                        }
                        else
                        {
                            intersect_0 = GetIntersectPoint();
                            pos_0 = next_pos;
                        }
                    }

                    mouse_pos_0 = Input.mousePosition;
                    break;
                case TouchPhase.Stationary:

                    break;
                case TouchPhase.Ended:
                    break;
                default:
                    break;
            }

        }

        if (force_move)
        {
            next_pos = Vector3.zero;
            forward_speed = 5.0f + 20 * game_variables_so.kick_force;
        }

        float x_lerp = Mathf.Lerp(transform.localPosition.x, next_pos.x, Time.deltaTime * smooth);
        float z_lerp = transform.parent.position.z + Time.deltaTime * forward_speed  ;
        transform.parent.position = new Vector3(0, transform.parent.position.y, z_lerp);
        transform.localPosition = new Vector3(x_lerp, 0, 0);

    }

    public Vector3 GetIntersectPoint()
    {

        float y_screen_point_offset = cam.WorldToScreenPoint(transform.position).y*1.5f;
        Ray cam_ray = cam.ScreenPointToRay(new Vector2(Input.mousePosition.x, y_screen_point_offset));

        Vector3 line1_point = cam_ray.origin;
        Vector3 line2_point = new Vector3(line1_point.x, 0, line1_point.z);

        Vector3 line1_dir = cam_ray.direction;
        Vector3 line2_dir = new Vector3(cam_ray.direction.x, 0, cam_ray.direction.z);

        //Debug.DrawRay(line1_point, line1_dir * 100, Color.black);
        //Debug.DrawRay(line2_point, line2_dir * 100, Color.red);

        Math3d.LineLineIntersection(out Vector3 intersection, line1_point, line1_dir, line2_point, line2_dir);

        return intersection;

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "finish_line_0")
        {
            forward_speed = 1;
            force_move = true;
        }

        if (other.tag == "finish_line_1")
        {
            play = false;
        }
    }


    public void Play()
    {
        play = true;
    }

    public void OnGameOver()
    {
        play = false;
    }



}
