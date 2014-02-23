using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FollowWaypoints : MonoBehaviour
{
    public Waypoint[] waypoints;
    private bool running;
    public float speed = 10.0f;
    private Vector3 lastPos;

    private int targetWaypointID;
    public Transform FishMesh;

    public bool reverse;

    public bool active = true;
    void Awake()
    {
        running = true;
        waypoints = GetComponentsInChildren<Waypoint>();
        if(waypoints.Length>0)
        {
           // transform.position = waypoints[0].transform.position;
        }
        lastPos = transform.position;
    }
    void Update()
    {
        if (active)
        {
            if(waypoints.Length == 0)
            {
                return;
            }
            Vector3 moved = lastPos - transform.position;
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i].transform.position += moved;
            }
                lastPos = transform.position;

            float distance = (waypoints[targetWaypointID].transform.position - transform.position).magnitude;
            float moveDistance = Mathf.Min(speed*Time.deltaTime,
                                           distance);

            Vector3 moveDir = (waypoints[targetWaypointID].transform.position - transform.position)/distance;
            if(reverse)
            {
                FishMesh.transform.forward = Vector3.RotateTowards(FishMesh.transform.forward,-moveDir,1.5f*Time.deltaTime,0.5f);
            }
            else
            {
                FishMesh.transform.forward = Vector3.RotateTowards(FishMesh.transform.forward, moveDir, 1.5f * Time.deltaTime, 0.5f);
            }

            transform.position += moveDir*moveDistance;
            if (distance <= 0.5f)
            {
                targetWaypointID++;
                if (targetWaypointID >= waypoints.Length)
                {
                    targetWaypointID -= waypoints.Length;
                }
            } 
        }
        
    }
    public void FindClosestWaypoint()
    {
        int closestWaypoint = 0;
        for (int i = 0; i < waypoints.Count(); i++)
        {
            if ((transform.position - waypoints[closestWaypoint].transform.position).sqrMagnitude < (transform.position - waypoints[i].transform.position).sqrMagnitude)
            {
                closestWaypoint = i;
            }
        }
        targetWaypointID = closestWaypoint;
    }
    void OnDrawGizmos()
    {
        waypoints = GetComponentsInChildren<Waypoint>();

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i >= 1)
            {
                Gizmos.DrawLine(waypoints[i - 1].transform.position, waypoints[i].transform.position);
            }
        }
        Gizmos.DrawLine(waypoints[Mathf.Max(waypoints.Length-1,0)].transform.position, waypoints[0].transform.position);
        Gizmos.DrawLine(transform.position,waypoints[targetWaypointID].transform.position);
    }
}

