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
            gotBounds = false;
            return;
        }
        if (outsideBounds)
        {
            schooling.Overridden = true;
            fishMove.targetDirection = boundsOctree.position -transform.position;
        }
        else
        {
            schooling.Overridden = false;
        }
        if(transform.position != oldPos)
        {
            if (outsideBounds)
            {
                BoundsOctree bo = baseOctree.GetBound(true, transform.position);

                if (bo != null && bo.Empty && bo.Related && !bo.Obstacle)
                {
                    outsideBounds = false;
                    boundsOctree = bo;
                }
                else
                {

                    outsideBounds = true;
                }
            }
            else
            {
                if (!boundsOctree.isPointInside(transform.position))
                {
                    outsideBounds = true;
                }
            }
            oldPos = transform.position;
        }
    }
#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        if (boundsOctree != null)
        {
            Gizmos.DrawCube(boundsOctree.position,Vector3.one*boundsOctree.size*2.0f);

            Gizmos.color = new Color(0.1f,0f,0.8f,0.2f);
            foreach (var neighbor in boundsOctree.neighbors)
            {

                Gizmos.DrawCube(neighbor.position, (Vector3.one * neighbor.size * 2) - Vector3.one * 0.1f);
                
            }
        }
        return;

    }
#endif

}