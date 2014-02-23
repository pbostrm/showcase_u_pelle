using UnityEngine;
using System.Collections;

public class FishHereMarkerManipulation : MonoBehaviour {
    public float RotationSpeed;
    public float BoppingSpeed;

    public float DistanceTolerance;

    private float _mult;
    private Vector3 startPos;
    private bool _reverse;
	// Use this for initialization
	void Start () {
        startPos = transform.position;
        _mult = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.RotateAround(Vector3.up, sTime.deltaTime * RotationSpeed);

        if (gameObject.transform.position.y >= startPos.y + DistanceTolerance)
            _reverse = true;
        else if (gameObject.transform.position.y <= startPos.y - DistanceTolerance)
            _reverse = false;

        gameObject.transform.position = new Vector3(transform.position.x, (_reverse ? transform.position.y - (sTime.deltaTime * _mult) * BoppingSpeed : transform.position.y + (sTime.deltaTime * _mult) * BoppingSpeed) , transform.position.z);
	}
}
