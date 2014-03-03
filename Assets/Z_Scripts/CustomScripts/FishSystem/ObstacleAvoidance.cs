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

    public float maximumObstacleDistance = 4.0f;
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
                            Vector3 v1 = (boundsOctree.position - neighBor.position);
                            Vector3 v2 = getFaceDirection((v1 * neighBor.size * 2)+boundsOctree.position, neighBor);   
                            
                            

                            Vector3 facePoint = boundsOctree.position + (-v2 * boundsOctree.size);
                            //getting corner
                            Vector3 v2isOne;
                            v2isOne.x = v2.x * v2.x;
                            v2isOne.y = v2.y * v2.y;
                            v2isOne.z = v2.z * v2.z;

                            if (v2isOne == Vector3.one) // special case if its a corner.
                            {
                                Vector3 repelDir = facePoint - transform.position;
                                if (repelDir.sqrMagnitude <= maximumObstacleDistance * maximumObstacleDistance)
                                {
                                    RepelVelocity += repelDir.normalized * (1.0f - (repelDir.magnitude / maximumObstacleDistance));
                                    gotObstacles = true;

                                }
                            }
                            else
                            {
                                Plane p1 = new Plane((facePoint - boundsOctree.position).normalized, facePoint);
                                float distance = p1.GetDistanceToPoint(transform.position);

                                Debug.DrawLine(facePoint, transform.position, Color.yellow);


                                if (distance < maximumObstacleDistance)
                                {
                                    Debug.DrawLine(transform.position, transform.position - (p1.normal * distance), Color.red);

                                    gotObstacles = true;

                                    RepelVelocity += v2.normalized * (1.0f - (distance / maximumObstacleDistance));
                                }

                            }
                        }
                    }
                    if (gotObstacles)
                    {
                        schooling.RepelVelocity = RepelVelocity;
                        schooling.GroupUpVelocity = Vector3.zero;
                        schooling.MatchVelocity = Vector3.zero;

                    }
                        
                }
            }
        }

    }
    Vector3 getFaceDirection(Vector3 point,BoundsOctree bo)
    {
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
    public void OnDrawGizmos()
    {
        if (gotObstacles && boundsOctree != null )
        {
            Gizmos.color = new Color(1f, 0f, 1f, 0.1f);
            Gizmos.DrawCube(boundsOctree.position,Vector3.one*boundsOctree.size*2);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(boundsOctree.position, Vector3.one * boundsOctree.size * 2);

            
        }
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
