using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class OctreeObstacle_Sphere: MonoBehaviour
{
    List<BoundsOctree> occupiedOctrees;

    public static int ObstacleSphereCnt = 0;
    int id;
    public int occupiedOctreesCnt;
    public bool InBoidsArea;

    public float radius;
    BoundsOctree boundsToBeDeGhosted;

    Vector3 startposition;

    public bool resettable;

    public void Awake()
    {
        id = ObstacleSphereCnt++;

        startposition = transform.position;
        occupiedOctrees = new List<BoundsOctree>();
        radius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
    }
    public void Update()
    {
        if (occupiedOctrees != null)
        {

            for (int i = occupiedOctrees.Count - 1; i >= 0; i--)
            {
                if (!occupiedOctrees[i].isSphereInside(transform.position, radius))
                {
                    occupiedOctrees[i].Obstacle = false;
                    boundsToBeDeGhosted = occupiedOctrees[i];
                    boundsToBeDeGhosted.CleanupObstacleGhost();
                    occupiedOctrees.RemoveAt(i);
               
                }
                //if(boundsToBeDeGhosted != null)
                  // boundsToBeDeGhosted.CleanupObstacleGhost();

            }
        }
        foreach (var boundsOctree in BoidsArea.boundsCollection)
        {
            boundsOctree.GetBounds(occupiedOctrees, transform.position, radius);
        }
        if (occupiedOctrees != null)
        {
            occupiedOctreesCnt = occupiedOctrees.Count;
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.4f, 0.6f, 0f, 0.2f);
        if (occupiedOctrees != null)
        {
            foreach (var occupiedOctree in occupiedOctrees)
            {
                Gizmos.DrawCube(occupiedOctree.position, occupiedOctree.size * Vector3.one * 2);
            }
        }
    }
    public void OnGUI()
    {
        if (resettable)
        {
            if (GUI.Button(new Rect(10, 130+(id*22), 100, 20), "ResetBall"))
            {
                transform.position = startposition;
            }
        }
        
    }
}
