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
        if (baseOctree != null)
        {
            Gizmos.color = new Color(0.1f,0.9f,0.1f,0.5f);
            Vector3 point = (transform.position - baseOctree.position);
            //int b_x = (int)(point.x+size);
            byte b_x = baseOctree.ConvertCartesianToBinary(point.x);
            byte b_y = ((byte)(((point.y + baseOctree.size) / (baseOctree.size * 2.0f)) * 256));
            byte b_z = ((byte)(((point.z + baseOctree.size) / (baseOctree.size * 2.0f)) * 256));


            BoundsOctree bO = baseOctree.GetBoundByBinary(transform.position);
            BoundsOctree bO2 = baseOctree.GetBound(true,transform.position);
            if(bO !=null && bO2 != null)
            {
                //DebugOutput.Shout("b_X = " + b_x + " " + b_y + " " + b_z + " returned: " + bO.binaryLocation_X + " " + bO.binaryLocation_Y + " " + bO.binaryLocation_Z + " correct " + bO2.binaryLocation_X + " " + bO2.binaryLocation_Y + " " + bO2.binaryLocation_Z);
                DebugOutput.Shout("b_X = " + b_x + " " + Convert.ToString(b_x, 2) + " returned: " + bO.binaryLocation_X + " " + Convert.ToString(bO.binaryLocation_X, 2) + " correct " + bO2.binaryLocation_X + " " + Convert.ToString(bO2.binaryLocation_X, 2));

                Gizmos.DrawCube(bO.position, (Vector3.one * bO.size * 2) - Vector3.one * 0.1f);

                Gizmos.color = new Color(0.9f, 0.4f, 0.0f, 0.99f);

                Gizmos.DrawCube(bO2.position, (Vector3.one * bO2.size * 2) - Vector3.one * 0.1f);
            }
        }
        base.sDrawGizmos();

    }
#endif

}