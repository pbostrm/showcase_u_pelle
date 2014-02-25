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

    public float radius;

    Vector3 startposition;
    Vector3 lastUpdatePos;
    public float smallestVolumeHalfSize;

    public bool resettable;
    public float createdOctreesPerSecond;
    public float createdOctreesPerFrameAverage;
    float createdOctreesCnt;
    float createdOctreesTimer;


    public void Awake()
    {
        id = ObstacleSphereCnt++;

        startposition = transform.position;
        occupiedOctrees = new List<BoundsOctree>();
        radius = GetComponent<SphereCollider>().radius * transform.lossyScale.x;
    }
    public void Update()
    {
        int octreesCreated = 0;
        createdOctreesTimer += Time.deltaTime;

        if (occupiedOctrees == null)
        {
            occupiedOctrees = new List<BoundsOctree>();
        }
        if (occupiedOctrees != null)
        {
            if ((transform.position - lastUpdatePos).sqrMagnitude > smallestVolumeHalfSize * smallestVolumeHalfSize)
            {
                lastUpdatePos = transform.position;
                for (int i = occupiedOctrees.Count - 1; i >= 0; i--)
                {
                    if (!occupiedOctrees[i].IntersectSphere(transform.position, radius))
                    {
                        occupiedOctrees[i].Obstacle = false;
                        occupiedOctrees[i].CleanupObstacleGhost();
                        occupiedOctrees.RemoveAt(i);
               
                    }
                    else
                    {
                        if (occupiedOctrees[i].neighbors != null)
                        {
                            for (int j = occupiedOctrees[i].neighbors.Count - 1; j >= 0; j--)
                            {
                                if (!occupiedOctrees[i].neighbors[j].Obstacle)
                                {

                                    occupiedOctrees[i].neighbors[j].GetBounds(false, occupiedOctrees, transform.position, radius, ref octreesCreated);

                                }
                            }
                        }
                    }
                }
            }
        }

        if (!BoidsArea.fastObstacle || occupiedOctrees.Count ==0)
        {
            if (BoidsArea.boundsCollection != null)
            {
                float volumeSize;
                foreach (var boundsOctree in BoidsArea.boundsCollection)
                {
                    volumeSize = boundsOctree.size / Mathf.Pow(2, boundsOctree.levelDepth);
                    if (volumeSize < smallestVolumeHalfSize || smallestVolumeHalfSize <= 0.0f)
                    {
                        smallestVolumeHalfSize = volumeSize;
                    }
                    boundsOctree.GetBounds(false,occupiedOctrees, transform.position, radius,ref octreesCreated);
                }
            }
        }

        createdOctreesCnt += octreesCreated;
        octreesCreated = 0;

        if (createdOctreesTimer >= 1.0f)
        {
            createdOctreesPerSecond = createdOctreesCnt;
            createdOctreesPerFrameAverage = createdOctreesPerSecond * Time.deltaTime;
            createdOctreesCnt = 0.0f;
            createdOctreesTimer = 0.0f;
        }
        if (occupiedOctrees != null)
        {
            occupiedOctreesCnt = occupiedOctrees.Count;
        }
        else
        {

            occupiedOctreesCnt = 0;
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
            if (GUI.Button(new Rect(10, 250+(id*22), 100, 20), "ResetBall"))
            {
                transform.position = startposition;
            }
        }
        
    }
}
