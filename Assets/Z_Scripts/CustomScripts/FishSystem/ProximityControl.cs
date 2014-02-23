using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ProximityControl : sBehaviour
{
    public float ProximityDetectionOuterRadius = 1.0f;
    public LayerMask ProximityMask;
    public LayerMask ObstacleMask;
    public Color32 outerProximityLineColor;
    public float pcCountdownInterval = 1.0f;
    private float pcCountdown;
    public List<FishControl> outerProximity = new List<FishControl>();
    public List<Collider> obstacles = new List<Collider>(); 
    private FishControl fishControl;
    public void Awake()
    {
        pcCountdown = UnityEngine.Random.Range(0.0f, pcCountdownInterval);
        fishControl = GetComponent<FishControl>();
    }
    public void Update()
    {
        if (pcCountdown <= 0.0f)
        {
            CheckProximity();
            pcCountdown += pcCountdownInterval;
        }
        pcCountdown -= sTime.deltaTime;

    }
    void CheckProximity()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,
            ProximityDetectionOuterRadius, ProximityMask);
        if (hitColliders.Length>=2)
        {
            //DebugOutput.Shout("Fish in proximity");
            AddFishToProximity(outerProximity,hitColliders);
            AddObstaclesToProximity(obstacles,hitColliders);
        }
        else
        {
            if (outerProximity.Count >= 1)
            {
                outerProximity.Clear();
            }
            if (obstacles.Count >= 1)
            {
                obstacles.Clear();
            }
            //DebugOutput.Shout("El nada del fish");
        }
    }
    void AddFishToProximity(List<FishControl> toList,Collider[] colliders)
    {
        toList.Clear();

        foreach (var collider1 in colliders)
        {
            FishControl control = collider1.GetComponent<FishControl>();
            if (control != null)
            {
                if (control != fishControl)
                {
                    toList.Add(control);
                }
            }
        }
    }
    void AddObstaclesToProximity(List<Collider> toList,Collider[] colliders)
    {
        toList.Clear();

        foreach (var collider1 in colliders)
        {
            if ((ObstacleMask.value & 1<< collider1.gameObject.layer) != 0)
            {
                obstacles.Add(collider1);
            }
        }
    }
#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        Gizmos.color = mainGizmoColor;

        Gizmos.DrawWireSphere(transform.position, ProximityDetectionOuterRadius);
        if (outerProximity.Count >= 1)
        {
            Gizmos.color = outerProximityLineColor;
            foreach (var control in outerProximity)
            {
                Gizmos.DrawLine(transform.position,control.transform.position);
            }
        }
    }
#endif
}
