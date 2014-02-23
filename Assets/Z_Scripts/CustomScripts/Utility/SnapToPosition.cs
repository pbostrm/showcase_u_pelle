using UnityEngine;
using System.Collections;

public class SnapToPosition : MonoBehaviour
{

    public bool lockXAxis;
    public bool lockYAxis;
    public bool lockZAxis;

    public float lockXAxisTo;
    public float lockYAxisTo;
    public float lockZAxisTo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (lockXAxis)
        {
            transform.position = new Vector3(lockXAxisTo,transform.position.y,transform.position.z);
        }
        if (lockYAxis)
        {
            transform.position = new Vector3(transform.position.x, lockYAxisTo, transform.position.z);
        }
        if (lockZAxis)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, lockZAxisTo);
        }

	}
}
