using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TranslationHandle : MonoBehaviour
{
    public bool toggled;
    bool lastUpdate;

    public bool movingThis;
    public LayerMask layermask;

    public Plane plane;

    public Vector3 axis;

    Vector3 mousePosOffset;

    public void Awake()
    {
    }
    public void Update()
    {
        toggled = Input.GetMouseButton(0);
        if (lastUpdate ^ toggled)
        {
            if (toggled)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 5.0f);

                if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000, layermask))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        mousePosOffset = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
                        movingThis = true;
                    }
                }
            }
            else
            {
                movingThis = false;
            }
            lastUpdate = toggled;

        }
        if (movingThis)
        {
            if (axis == Vector3.one)
            {
                plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);

            }
            else
            {
                plane.SetNormalAndPosition(transform.right, transform.position);
            }
            Debug.DrawLine(transform.position, transform.position + plane.normal*3.0f, Color.cyan);
               
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition+mousePosOffset);

            float distance;
            if(plane.Raycast(ray,out distance))
            {
                Vector3 point = ray.GetPoint(distance);
                point.x = point.x*axis.x +transform.position.x*-(axis.x-1);
                point.y = point.y*axis.y+transform.position.y*-(axis.y-1);
                point.z = point.z*axis.z+transform.position.z*-(axis.z-1);


                transform.parent.position = point;

            }
        }
    }
}