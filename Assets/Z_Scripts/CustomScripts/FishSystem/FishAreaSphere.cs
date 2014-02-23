using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FishAreaSphere : MonoBehaviour
{
    public string FishAreaName = "fishOne";
    public float Radius;
    //thihi spawn some fishies
    public GameObject fishPrefab;
    public int maxSpawn = 4;
    public int minSpawn = 1;

    public static List<FishAreaSphere> FishAreaSpheres = new List<FishAreaSphere>();

    public void Start()
    {
        FishAreaSpheres.Add(this);
        int r = UnityEngine.Random.Range(minSpawn, maxSpawn);
        while (r > 0)
        {
            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/FishOne"));
            g.transform.parent = transform;
            g.transform.localPosition = new Vector3(UnityEngine.Random.Range(-Radius * 0.5f, Radius * 0.5f), UnityEngine.Random.Range(-Radius * 0.5f, Radius * 0.5f), UnityEngine.Random.Range(-Radius * 0.5f, Radius * 0.5f));
            r--;
        }
        
    }

    public static Vector3 insideFishArea(Vector3 pos)
    {
        if (FishAreaSpheres != null && FishAreaSpheres.Count > 0)
        {
            float smallestSqrDistance = (FishAreaSpheres[0].transform.position - pos).sqrMagnitude;
            Vector3 closestPos = FishAreaSpheres[0].transform.position;
            foreach (var fishAreaSphere in FishAreaSpheres)
            {
                float f = (fishAreaSphere.transform.position - pos).sqrMagnitude;
                if (f <= fishAreaSphere.Radius * fishAreaSphere.Radius)
                {
                    return Vector3.zero;
                }
                else
                {
                    if (f-fishAreaSphere.Radius*fishAreaSphere.Radius < smallestSqrDistance)
                    {
                        smallestSqrDistance = f-fishAreaSphere.Radius*fishAreaSphere.Radius;
                        closestPos = fishAreaSphere.transform.position;
                    }
                }
            }
            //Debug.Log("Poop!");
            return closestPos;
        }


        return Vector3.zero;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.color = new Color(0.8f,1.7f,0.0f,0.2f);
        Gizmos.DrawSphere(transform.position,Radius);
    }
}