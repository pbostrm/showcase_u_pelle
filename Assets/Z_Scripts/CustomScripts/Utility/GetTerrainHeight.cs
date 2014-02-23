using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class GetTerrainHeight
{
    public static float GetHeightAt(Vector3 pos)
    {
        LayerMask layerMask;
        layerMask = LayerMask.NameToLayer("Terrain");

        if (layerMask != null)
        {
            Ray ray = new Ray(pos, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000.0f))
            {
                return hit.point.y;
            }
            else
            {
                ray.direction = Vector3.up;
                if (Physics.Raycast(ray, out hit, 10000.0f))
                {
                    return hit.point.y;

                }
                else
                {
                    //Debug.Log(ray.direction);
                }
            }
        }
        return pos.y;
    }
}