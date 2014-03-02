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

    Vector3 debugObstacleFace;
    static public bool toggleDebugObstacle;
    bool gotObstacles;
    public void Awake()
    {
        fishControl = GetComponent<FishControl>();

        fishMove = GetComponent<FishMove>();
        schooling = GetComponent<Schooling>();
        updateTimer = UnityEngine.Random.Range(0.0f, UpdateInterval);

        GL_OctreeRenderer.AddRenderDelegate(GL_Draw);

    }

    public void Update()
    {


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
                
            if(boundsOctree !=null && boundsOctree.Border)
            {
                if (boundsOctree.neighbors != null)
                {
                    RepelVelocity = Vector3.zero;
                    gotObstacles = false;
                    foreach (var neighBor in boundsOctree.neighbors)
                    {
                        if (!neighBor.Empty || neighBor.Obstacle)
                        {
                            gotObstacles = true;
                            Vector3 v1 = (boundsOctree.position - neighBor.position);
                            Vector3 v2 = getFaceDirection(v1 * neighBor.size * 2, neighBor);    //v1*neighbor.size*2 to make sure it is outside the cube.

                            float magnitude = Mathf.Clamp(distanceToOctree(boundsOctree, -v2),0.01f,boundsOctree.size);
                            RepelVelocity += v2 *1.0f / Mathf.Clamp(magnitude/boundsOctree.size,0.01f,1.0f);
                           
                        }
                    }
                    if (gotObstacles)
                    {
                        fishMove.moveDirection = RepelVelocity.normalized;
                        schooling.RepelVelocity = RepelVelocity;                      
                    }
                        
                }
            }
        }

    }
    float distanceToOctree(BoundsOctree currentOctree, Vector3 direction)
    {
        Vector3 facePos = currentOctree.position + (direction * currentOctree.size);
        Vector3 difference = facePos - transform.position;
        Debug.DrawLine(facePos, facePos - difference, Color.red);
        Debug.DrawLine(currentOctree.position, facePos, Color.blue);
        debugObstacleFace = facePos;
        return difference.magnitude; //yes sqrmagnitude is faster but this code is not yet done so i havent put in enough brainpower on this issue.
    }
    Vector3 getFaceDirection(Vector3 point,BoundsOctree bo)
    {
        //i could probably do this by reversing the BoundsOctree.RefreshNeighbors last loop, that way i would get all the edges and corners.
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

    public void GL_Draw()
    {
        if (gotObstacles && boundsOctree != null && toggleDebugObstacle)
        {
            GL.Color(Color.red);
            GL.Vertex(transform.position);
            GL.Vertex(debugObstacleFace);
            GL.Color(Color.blue);

            GL.Vertex(debugObstacleFace);
            GL.Vertex(boundsOctree.position);
        }
     
    }
}
