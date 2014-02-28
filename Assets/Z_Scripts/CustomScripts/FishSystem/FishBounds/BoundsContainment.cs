using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class BoundsContainment  : sBehaviour
{
    private FishControl fishControl;
    private FishMove fishMove;
    private Schooling schooling;
    public BoundsOctree baseOctree;
    public BoundsOctree boundsOctree;
    public float updateInterval = 0.2f;
    public float updateTimer;

    private bool ObstacleAwareness;
    private bool newDirection;
    private Vector3 overrideDirection;

    public float scanRange = 0.5f;
    public Vector3 hitNormal;
    private RaycastHit hit;

    public Vector3 oldPos;

    public bool outsideBounds;

    public bool gotBounds = true;
    public void Awake()
    {
        fishControl = GetComponent<FishControl>();
        fishMove = GetComponent<FishMove>();
        schooling = GetComponent<Schooling>();
        updateTimer = UnityEngine.Random.Range(0.0f, updateInterval);

        hit = new RaycastHit();
    }
    public void Update()
    {
        if (boundsOctree == null)
        {
            boundsOctree = BoidsArea.getBoundsOctree( transform.position);

            if (boundsOctree == null)
            {
                outsideBounds = true;
                gotBounds = false;

            }
        }

        if (boundsOctree != null)
        {
            if (!boundsOctree.isPointInside(transform.position) || boundsOctree.Obstacle)
            {
                boundsOctree = null;
            }
            else
            {
                oldPos = transform.position;

            }
            outsideBounds = false;
        }
        if (outsideBounds)
        {
            schooling.Overridden = true;
            fishMove.targetDirection = oldPos -transform.position;
        }
        else
        {
            schooling.Overridden = false;
        }
    
    }
#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        if (boundsOctree != null)
        {
            Gizmos.DrawCube(boundsOctree.position,Vector3.one*boundsOctree.size*2.0f);

            Gizmos.color = new Color(0.1f,0f,0.8f,0.2f);
            if (boundsOctree.neighbors != null)
            {
                foreach (var neighbor in boundsOctree.neighbors)
                {

                    Gizmos.DrawCube(neighbor.position, (Vector3.one * neighbor.size * 2) - Vector3.one * 0.1f);

                }
            }
            
        }
        return;

    }
#endif

}