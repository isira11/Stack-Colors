using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public AnimationCurve heigthVSfov;
    public AnimationCurve heigthVSz;
    public AnimationCurve heigthVSy;
    public Camera still_cam; 
    public Transform target;
    public Transform bag;
    public float smooth = 5;


    Camera cam;


    int child_count = -1;
    Bounds bounds;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
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

        if (bag.childCount % 10 == 0 && child_count!= bag.childCount)
        {
            child_count = bag.childCount;
            bounds = CalculateBounds();
        }

        float fov = heigthVSfov.Evaluate(bounds.size.y);
        float y = heigthVSy.Evaluate(bounds.size.y);
        float z = heigthVSz.Evaluate(bounds.size.y);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView ,fov,Time.deltaTime*5);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, y, z),Time.deltaTime*5);



        //still_cam.transform.localPosition = transform.localPosition;
        //still_cam.fieldOfView = cam.fieldOfView;


    }

    public Bounds CalculateBounds()
    {
        Bounds b = bag.parent.GetComponent<MeshRenderer>().bounds;

        foreach (Transform item in bag)
        {
            b.Encapsulate(item.GetComponent<MeshRenderer>().bounds);
        }

        return b;


    }
}
