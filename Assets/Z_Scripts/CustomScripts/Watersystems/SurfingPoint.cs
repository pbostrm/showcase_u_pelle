using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SurfingPoint : MonoBehaviour
{
    public float YOffset = 0f;
    public void Update()
    {
        transform.position = new UnityEngine.Vector3(transform.position.x, Watersystem.GetAltitudeAt(transform.position) + YOffset, transform.position.z);
    }
}
