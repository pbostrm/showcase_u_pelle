using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class RotateAroundAxis : MonoBehaviour
{
    public Vector3 Axis;
    public float speed;

    public void Update()
    {
        transform.RotateAroundLocal(Axis,speed*sTime.deltaTime);
    }
}