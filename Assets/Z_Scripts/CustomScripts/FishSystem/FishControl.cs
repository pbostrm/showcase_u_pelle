using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
class FishControl : sBehaviour
{
    public static int idCnt;
    public static List<FishControl> fishCollection = new List<FishControl>();
    public int FishID;

    public float MoveSpeed = 0.5f;
    public Vector3 RotationalSpeed;
    public float YawSpeed = 1.0f;
    public float RollSpeed = 1.0f;
    public float PitchSpeed = 1.0f;

    public delegate int Comparison<FishControl>(FishControl first, FishControl second);

    public FishMove fishMove;
    public AttractivenessModule attractivenessModule;

    public string Nickname;

    public void Awake()
    {

        FishID = idCnt++;
        name = name + " " + FishID.ToString();
        fishCollection.Add(this);
        fishMove = GetComponent<FishMove>();
        attractivenessModule = GetComponent<AttractivenessModule>();
    }
    public void Start()
    {
        //Nickname = FishSchoolObservation.nickNames[0];
        //FishSchoolObservation.nickNames.RemoveAt(0);
    }
#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,0.1f);
    }
#endif

    public int Compare(FishControl first,FishControl second)
    {
        if ((transform.position - first.transform.position).sqrMagnitude <= (transform.position - second.transform.position).sqrMagnitude)
        {
            return -1;
        }
        return +1;
    }
    
}
