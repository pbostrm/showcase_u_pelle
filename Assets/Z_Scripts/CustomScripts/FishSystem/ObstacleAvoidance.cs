using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ObstacleAvoidance : sBehaviour
{
    private FishControl fishControl;
    private FishMove fishMove;
    private Schooling schooling;

    float updateTimer;
    public float UpdateInterval;

    public Vector3 RepelVelocity;

    BoundsOctree boundsOctree;
    public void Awake()
    {
        fishControl = GetComponent<FishControl>();

        fishMove = GetComponent<FishMove>();
        schooling = GetComponent<Schooling>();
        updateTimer = UnityEngine.Random.Range(0.0f, UpdateInterval);


    }

    public void Update()
    {
      //  if (updateTimer <= 0)
        {
            updateTimer += UpdateInterval;

            if(boundsOctree==null)
            {
                boundsOctree = BoidsArea.getBoundsOctree(transform.position);
            }

            if (boundsOctree != null)
            {
                if(!boundsOctree.isPointInside(transform.position))
                {
                    boundsOctree = BoidsArea.getBoundsOctree(transform.position);
                }

                if(boundsOctree !=null)
                {
                    if (boundsOctree.neighbors != null)
                    {
                        RepelVelocity = Vector3.zero;
                        bool gotObstacles = false;
                        foreach (var neighBor in boundsOctree.neighbors)
                        {
                            if (!neighBor.Empty || neighBor.Obstacle)
                            {
                                gotObstacles = true;
                                Vector3 v1 = (boundsOctree.position - neighBor.position);
                                Vector3 v2 = getFaceDirection(v1 * neighBor.size * 2, neighBor);    //v1*neighbor.size*2 to make sure it is outside the cube.
                                //Vector3 v3 = v2 * neighBor.size;
                                float magnitude = (boundsOctree.position - transform.position).sqrMagnitude
                                    / (boundsOctree.position - v2 * neighBor.size).sqrMagnitude;
                                RepelVelocity += v2 *1.0f / Mathf.Clamp(magnitude,0.0f,1.0f);
                                //RepelVelocity += direction*force multiplier;
                            }
                        }
                        if (gotObstacles)
                        {
                            fishMove.moveDirection = RepelVelocity.normalized;
                            schooling.RepelVelocity = RepelVelocity;
                            //schooling.GroupUpVelocity = Vector3.zero;
                            //schooling.MatchVelocity = Vector3.zero;
                        }
                        
                    }
                }
            }
            
        }
        updateTimer -= Time.deltaTime;
    }

    Vector3 getFaceDirection(Vector3 point,BoundsOctree bo)
    {
        //i could probably do this by reversing the BoundsOctree.RefreshNeighbors, last loop that way i would get all the edges and corners.
        Vector3 dir = new Vector3();
        if (point.y >= bo.top.y)
        {
            dir.y = 1;
        }
        else if ( point.y < bo.bottom.y)
        {
            dir.y = -1;
        }

        if (point.x >= bo.east.x )
        {
            dir.x = 1;
        }
        else if ( point.x < bo.west.x)
        {
            dir.x = -1;
        }

        if (point.z >= bo.north.z )
        {
            dir.z = 1;
        }
        else if ( point.z < bo.south.z)
        {
            dir.z = -1;
        }
        return dir;
    }
}
