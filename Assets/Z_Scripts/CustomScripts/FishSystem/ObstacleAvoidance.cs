using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ObstacleAvoidance : sBehaviour
{
    private FishControl fishControl;
    private ProximityControl proxControl;
    private FishMove fishMove;
    private Schooling schooling;

    public float updateInterval = 0.2f;
    public float updateTimer;

    private bool ObstacleAwareness;
    private bool newDirection;
    private Vector3 overrideDirection;

    public float scanRange = 0.5f;
    public Vector3 hitNormal;
    private RaycastHit hit;
    public void Awake()
    {
        fishControl = GetComponent<FishControl>();
        proxControl = GetComponent<ProximityControl>();
        fishMove = GetComponent<FishMove>();
        schooling = GetComponent<Schooling>();
        updateTimer = UnityEngine.Random.Range(0.0f, updateInterval);

        hit = new RaycastHit();
    }

    public void Update()
    {
        if (proxControl.obstacles.Count >= 1)
        {
            ObstacleAwareness = true;
            if (updateTimer <= 0.0f )
            {

                if (ScanDirection(transform.forward, scanRange))
                {
                    newDirection = true;
                    //DebugOutput.Shout("MazelTov!");
                    //Find a new route.
                    
                    overrideDirection = (hit.point + Vector3.Reflect(transform.forward, hit.normal) * hit.distance)
                        - transform.position;
                    int cnt = 0;

                    if (ScanDirection(overrideDirection, scanRange))
                    {
                        bool foundDirection = false;
                        while (!foundDirection)
                        {
                            cnt++;
                            overrideDirection = Vector3.Reflect(overrideDirection, hit.normal)*hit.distance;
                            if (overrideDirection == Vector3.zero)
                            {
                                overrideDirection = -transform.forward;
                            }
                            if (!ScanDirection(overrideDirection, scanRange))
                            {
                                foundDirection = true;
                                //DebugOutput.Shout("found an angle at: " + cnt.ToString());
                            }
                            if (cnt >= 100 )
                            {
                                //DebugOutput.Shout("Megabajs");
                                foundDirection = true;
                            }
                        }
                        //DebugOutput.Shout("BAJS!");
                    }
                    /*fishMove.targetDirection = (hit.point + Vector3.Reflect(transform.forward, hit.normal)*hit.distance) 
                        - transform.position;*/
                }
                else
                {
                    updateTimer += updateInterval;

                }


            }
            if (newDirection)
            {
                schooling.Overridden = true;
                if (Vector3.Angle(transform.forward, overrideDirection) >= 1.0f)
                {
                    fishMove.targetDirection = overrideDirection;
                }
                else
                {
                    newDirection = false;
                    schooling.Overridden = false;
                }
            }
            updateTimer -= Time.deltaTime;
        }
        else
        {
            ObstacleAwareness = false;
        }
    }
    public bool ScanDirection(Vector3 direction,float length)
    {

        if (Physics.SphereCast(transform.position,0.2f, direction,out hit, length, proxControl.ObstacleMask))
        {
            return true;
        }
        return false;
    }
#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        if (ObstacleAwareness)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * scanRange);

           /* Vector3 v1 = hit.point + hit.normal*scanRange;
            Gizmos.DrawLine(hit.point,v1);

            Gizmos.color = Color.cyan;
            Vector3 v2 = Vector3.Cross(v1, transform.forward);
            Gizmos.DrawLine(v1 ,v1+v2);

            Gizmos.color = Color.blue;
            Vector3 v3 = Vector3.Cross(v1, v2);
            Gizmos.DrawLine(v1, v1 + v3);
            */
            Gizmos.color = Color.yellow;
            Vector3 v4 = Vector3.Reflect(transform.forward, hit.normal);
            Gizmos.DrawLine(hit.point, hit.point + v4*scanRange);

            Gizmos.color = Color.white;
            //Gizmos.DrawLine(transform.position, hit.point + v4 * hit.distance);
            Gizmos.DrawLine(transform.position,transform.position+overrideDirection );
        }
    }
#endif


}
