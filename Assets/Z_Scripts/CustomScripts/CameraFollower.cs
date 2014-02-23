using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CameraFollower : MonoBehaviour
{
    //public Transform target;
    public float targetDistanceSQR;
    public float targetDistance = 100;
    public float cameraHeight = 50.0f;
    public float smoothing = 100.0f;
    private bool running;

    private bool isLocal;

    public Transform cameraAnchor;
    public Vector3 lastPos;
    void Awake()
    {
        if (transform.parent != null)
        {
            isLocal = true;

            if (cameraAnchor == null)
            {
                cameraAnchor = GetComponentInChildren<SmoothLookAt>().transform;
            }
        }
        running = true;

    }
    void LateUpdate()
    {
        if (isLocal)
        {
            Vector3 direction = (transform.parent.position - lastPos);

            float distance = direction.magnitude;
            if (distance > targetDistance)
            {
                //move to target;
                transform.position = transform.parent.position - direction.normalized * (targetDistance + (distance - targetDistance) * Time.deltaTime);
                //transform.position = transform.position + (target.position - transform.position) * 0.3f * Time.deltaTime;

            }
            else if (distance < targetDistance)
            {

                //transform.position = transform.position - (target.position - transform.position)*0.3f * Time.deltaTime;
                transform.position = transform.parent.position - direction.normalized * (targetDistance + (distance - targetDistance) * Time.deltaTime);

            }
            lastPos = transform.position;

            transform.eulerAngles = Vector3.zero;
            transform.forward = (transform.parent.position - transform.position).normalized;
        }
        /*
        Vector3 direction = (target.position - transform.position);

        float distance = direction.magnitude;
        if (distance > targetDistance )
        {
            //move to target;
            transform.position = target.position - direction.normalized*(targetDistance + (distance - targetDistance) * Time.deltaTime);
            //transform.position = transform.position + (target.position - transform.position) * 0.3f * Time.deltaTime;

        }
        else if (distance < targetDistance )
        {

            //transform.position = transform.position - (target.position - transform.position)*0.3f * Time.deltaTime;
            transform.position = target.position - direction.normalized * (targetDistance + (distance - targetDistance) * Time.deltaTime);
            
        }*/
    }
    void OnDrawGizmos()
    {

        if (cameraAnchor != null)
        {
            Gizmos.DrawLine(transform.position, cameraAnchor.position);
            Gizmos.DrawLine(cameraAnchor.position, transform.parent.position);

        }
        /*
        if (!running)
        {
            Vector3 direction = (target.position - transform.position);

            float distance = direction.magnitude;
            if (distance > targetDistance)
            {
                //move to target;
                transform.position = target.position - direction.normalized * (targetDistance + (distance - targetDistance) * Time.deltaTime);
                //transform.position = transform.position + (target.position - transform.position) * 0.3f * Time.deltaTime;

            }
            else if (distance < targetDistance)
            {

                //transform.position = transform.position - (target.position - transform.position)*0.3f * Time.deltaTime;
                transform.position = target.position - direction.normalized * (targetDistance + (distance - targetDistance) * Time.deltaTime);

            }
        }
        if (GetComponentInChildren<Camera>() != null)
        {
            Vector3 camLocalPos = GetComponentInChildren<Camera>().transform.localPosition;
            camLocalPos.y = cameraHeight;
            GetComponentInChildren<Camera>().transform.localPosition = camLocalPos;
            Gizmos.color = new Color(0.8f,0.3f,0.0f);
            Gizmos.DrawLine(transform.position,target.position);
            Gizmos.DrawLine(transform.position,GetComponentInChildren<Camera>().transform.position);
        }
        */

    }
}