using UnityEngine;
using System.Collections;

public class TestingDistance : MonoBehaviour
{

    public float Distance = 1.0f;
    public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnDrawGizmos()
    {
        if (target != null)
        {
            if (QuickCompare.withinDistance(transform.position, target.transform.position, Distance))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position,target.transform.position);
        }
    }
}
