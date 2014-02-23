using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Waypoint : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,1.5f);
    }
}

