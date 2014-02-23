using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Schooling : sBehaviour
{
    public bool Boids = true;
    public bool Overridden;


    private FishControl fishControl;
    private ProximityControl proxControl;
    private FishMove fishMove;
    private AttractivenessModule attractivenessModule;
    public List<FishControl> closestFishes = new List<FishControl>();
    public float closestFishTimerInterval = 0.7f;

    private float closestFishTimer = 0.7f;

    public int maxCloseCount = 5;

    public float personalSpaceDistanceSqr = 5.0f;
    public Color32 personalSpaceColor;

    public float RepelFactor = 1.0f;
    public float GroupUpFactor = 1.0f;
    public float MatchVelocityFactor = 1.0f;

    Vector3 Velocity;
    public Vector3 RepelVelocity = new Vector3();
    public Vector3 GroupUpVelocity = new Vector3();
    public Vector3 MatchVelocity = new Vector3();


    public void Awake()
    {
        fishControl = GetComponent<FishControl>();
        proxControl = GetComponent<ProximityControl>();
        fishMove = GetComponent<FishMove>();
        if (FishAdministration._enAttractiveness)
            attractivenessModule = GetComponent<AttractivenessModule>();
        closestFishTimer = UnityEngine.Random.Range(0.0f, closestFishTimerInterval);
    }
    public void Update()
    {
        if (!Overridden)
        {
            if (closestFishTimer <= 0.0f)
            {
                //Sort
                closestFishes.Clear();
                closestFishes.AddRange(proxControl.outerProximity);
                closestFishes.Sort(new Comparison<FishControl>(fishControl.Compare));
                if (closestFishes.Count > maxCloseCount)
                {
                    closestFishes.RemoveRange(maxCloseCount, closestFishes.Count - maxCloseCount);
                }
                //Pick out closest
                closestFishTimer += closestFishTimerInterval;
            }
            closestFishTimer -= sTime.deltaTime;

            if (closestFishes.Count >= 1)
            {

                Velocity = (RepelVelocity * RepelFactor) + (GroupUpVelocity * GroupUpFactor) + (MatchVelocity * MatchVelocityFactor);

                fishMove.targetDirection = Velocity;
                float attractivenessOffset = 0.2f;
                Vector3 v1,v2,v3;
                // Repel!
                RepelVelocity = Vector3.zero;
                v2 = transform.position;
                v3 = fishMove.moveDirection;// *(fishControl.attractivenessModule.attractivenessRatio + attractivenessOffset);

                int aboveCnt = 0;

                foreach (var closestFish in closestFishes)
                {
                    if (closestFish.attractivenessModule != null )
                    {
                        //repel
                        v1 = (transform.position - closestFish.transform.position);
                        RepelVelocity += v1.normalized * 1.0f / v1.sqrMagnitude;

                        float AttractivenessModifier;
                        if (closestFish.attractivenessModule.attractivenessRatio > fishControl.attractivenessModule.attractivenessRatio)
                        {
                            aboveCnt++;
                            
                            AttractivenessModifier = 1.0f + (closestFish.attractivenessModule.attractivenessRatio - fishControl.attractivenessModule.attractivenessRatio);
                        }
                        else
                        {
                            AttractivenessModifier = 1.0f - (fishControl.attractivenessModule.attractivenessRatio - closestFish.attractivenessModule.attractivenessRatio);
                        }

                        //AttractivenessModifier = (closestFish.attractivenessModule.attractivenessRatio - fishControl.attractivenessModule.attractivenessRatio) / fishControl.attractivenessModule.attractivenessRatio;
                        //Group up
                        v2 = v2 + (transform.position + ((closestFish.transform.position - transform.position) * AttractivenessModifier));
                        //match velocity
                        v3 += closestFish.fishMove.moveDirection * AttractivenessModifier;
                    }
                    else
                    {
                        //repel
                        v1 = (transform.position - closestFish.transform.position);
                        RepelVelocity += v1.normalized * 1.0f / v1.sqrMagnitude;
                        //group up
                        v2 = v2 + closestFish.transform.position;

                        //match velocity
                        v3 += closestFish.fishMove.moveDirection;
                    }
                    

                }
                // Group up

                GroupUpVelocity = (v2 / (closestFishes.Count + 1)) - transform.position;//(transform.position - v1);
                //GroupUpVelocity = (v2 / (aboveCnt + 1)) - transform.position;//(transform.position - v1);
                // Match velocity

                MatchVelocity = v3.normalized * fishControl.MoveSpeed;

                
            }
        }
    }

#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        foreach (var closestFish in closestFishes)
        {
            if (closestFish.attractivenessModule.attractivenessRatio > fishControl.attractivenessModule.attractivenessRatio)
            {
                Gizmos.color = closestFish.attractivenessModule.triangleColor;
            }
            else
            {
                Gizmos.color = Color.gray;
            }
            Gizmos.DrawWireCube(closestFish.transform.position,Vector3.one*0.2f);
            Gizmos.DrawLine(transform.position,closestFish.transform.position);
        }
        if (closestFishes.Count >= 1)
        {
            Gizmos.DrawWireSphere(transform.position,(transform.position-closestFishes[0].transform.position).magnitude);
        }
        Gizmos.color = personalSpaceColor;
        Gizmos.DrawSphere(transform.position,Mathf.Sqrt(personalSpaceDistanceSqr));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,transform.position+RepelVelocity*RepelFactor);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + GroupUpVelocity*GroupUpFactor);
        Gizmos.DrawSphere(transform.position + GroupUpVelocity,0.01f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + MatchVelocity*MatchVelocityFactor);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + Velocity);

    }
#endif


}
