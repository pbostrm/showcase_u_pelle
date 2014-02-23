using UnityEngine;
using System.Collections;
using UnityEngine;

public class FishMove : sBehaviour
{
    public bool gotTarget = true;
    public Vector3 targetPosition;
    public Vector3 targetDirection;
    public Vector3 moveDirection;
    private FishControl fishControl;

    public float changeDirectionInterval = 5.0f;
    private float changeDirectionTimer;
    private bool firstRun = true;

    private Vector3 newForward;
    private Vector3 tDNorm;
    public void Awake()
    {
        changeDirectionTimer = Random.Range(0.0f, changeDirectionInterval);
        targetDirection = Random.rotation.eulerAngles.normalized * 2 - Vector3.one;
        transform.forward = Vector3.forward;
        moveDirection = transform.forward;
        transform.LookAt(Vector3.forward);
        fishControl = GetComponent<FishControl>();
    }
    public void Update()
    {
        if (changeDirectionTimer <= 0.0f)
        {
            targetDirection = Random.rotation.eulerAngles.normalized*2 - Vector3.one;

            //DebugOutput.Shout(name + " has a new direction " + targetDirection.ToString());

            changeDirectionTimer += changeDirectionInterval;
        }
        changeDirectionTimer -= sTime.deltaTime;

        //fix rotation
        tDNorm = targetDirection.normalized;

        newForward = transform.forward;
        
        
        newForward.x = Vector3.RotateTowards(newForward, tDNorm, fishControl.RotationalSpeed.x * sTime.deltaTime, 1.0f).x;
        newForward.y = Vector3.RotateTowards(newForward, tDNorm, fishControl.RotationalSpeed.y * sTime.deltaTime, 1.0f).y;
        newForward.z = Vector3.RotateTowards(newForward, tDNorm, fishControl.RotationalSpeed.z * sTime.deltaTime, 1.0f).z;
        
        //fix position

        if (newForward == Vector3.zero)
        {
            newForward = Random.rotation.eulerAngles.normalized * 2 - Vector3.one;
        }
        moveDirection = newForward;

        transform.forward = newForward;

        
        transform.position += moveDirection * fishControl.MoveSpeed * sTime.deltaTime;
        
    }
#if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + targetDirection * 1.0f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.0f);
    }
#endif

}
