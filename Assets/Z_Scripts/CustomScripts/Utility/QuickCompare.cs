using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

internal class QuickCompare
{
    public static bool withinDistance(Vector3 from, Vector3 to, float distance)
    {
        if ((to-from).magnitude > distance)
        {
            return false;
        }
        return true;
    }
    public static bool withinDistanceSqr(Vector3 from,Vector3 to,float distanceSqr)
    {
        if ((to - from).sqrMagnitude > distanceSqr)
        {
            return false;
        }
        return true;
    }
    public static float Distance(Transform from, Transform to)
    {
        return (from.position - to.position).magnitude;
    }
}
