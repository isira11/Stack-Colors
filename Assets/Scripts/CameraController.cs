using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public  float smooth = 5;


    private void Update()
    {
        Vector3 line1_point = new Vector3(transform.position.x, 0, transform.position.z) - Vector3.left * 100 / 2;
        Vector3 line1_dir = Vector3.left * 100;
        Vector3 line2_point = new Vector3(target.position.x, 0, target.position.z);
        Vector3 line2_dir = -new Vector3(transform.forward.x, 0, transform.forward.z) * 100;

        Debug.DrawRay(line1_point, line1_dir, Color.red);
        Debug.DrawRay(line2_point, line2_dir, Color.blue);


        Math3d.LineLineIntersection(out Vector3 intersection, line1_point, line1_dir, line2_point, line2_dir);

        Vector3 new_pos = new Vector3(intersection.x, transform.position.y, intersection.z);

        transform.position = Vector3.Lerp(transform.position, new_pos, Time.deltaTime * smooth);
    }
}
