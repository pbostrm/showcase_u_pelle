using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Surfing : MonoBehaviour
{
    public bool Smooth = true;
    public float pitchSmoothing = 1.0f;
    public float rollSmoothing = 1.0f;
    public float buoyancy = 1.0f; 
    public Vector3 size;
    public bool useRotation = true;

    public Vector3 targetPosition;
    #region Corners
   
    private Vector3 _frontCorner;
    private Vector3 frontCorner
    {
        get
        {
            _frontCorner.x = 0;
            _frontCorner.z = size.z*0.5f;
            return _frontCorner;
        }
        set { _frontCorner = value; }
        
    }


    float frontCorner_y
    {
        get { return _frontCorner.y; }
        set { _frontCorner.y = value; }

    }

    private Vector3 _backCorner;
    private Vector3 backCorner
    {
        get
        {
            _backCorner.x = 0;
            _backCorner.z = -size.z * 0.5f;
            return _backCorner;
        }
        set { _backCorner = value; }

    }
    private float backCorner_y
    {
        get { return _backCorner.y; }
        set { _backCorner.y = value; }

    }

    private Vector3 _leftCorner;
    private Vector3 leftCorner
    {
        get
        {
            _leftCorner.x = -size.x * 0.5f;
            _leftCorner.z = 0;
            return _leftCorner;
        }
        set { _leftCorner = value; }

    }
    private float leftCorner_y
    {
        get { return _leftCorner.y; }
        set { _leftCorner.y = value; }

    }


    private Vector3 _rightCorner;
    private Vector3 rightCorner
    {
        get
        {
            _rightCorner.x = size.x * 0.5f;
            _rightCorner.z = 0;
            return _rightCorner;
        }
        set { _rightCorner = value; }

    }

    private float rightCorner_y
    {
        get { return _rightCorner.y; }
        set { _rightCorner.y = value; }

    }
 #endregion Corners
    private Vector3 wFrontCorner;
    public Vector3 wBackCorner;
    private Vector3 wLeftCorner;
    private Vector3 wRightCorner;
    private Vector3 targetAngles;
    private Vector3 newAngles;
    private float yaw;
    private CharacterController cC;

    void Awake()
    {
        cC = GetComponent<CharacterController>();
    }
    void Update()
    {

        Matrix4x4 mat = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        //Gizmos.matrix = mat;

        if (useRotation)
        {
            wFrontCorner = mat.MultiplyPoint(frontCorner);
            wFrontCorner.y = Watersystem.GetAltitudeAt(wFrontCorner);
            wBackCorner = mat.MultiplyPoint(backCorner);
            wBackCorner.y = Watersystem.GetAltitudeAt(wBackCorner);
            wLeftCorner = mat.MultiplyPoint(leftCorner);
            wLeftCorner.y = Watersystem.GetAltitudeAt(wLeftCorner);
            wRightCorner = mat.MultiplyPoint(rightCorner);
            wRightCorner.y = Watersystem.GetAltitudeAt(wRightCorner);

            Vector3 pos = transform.position;
            pos.y = (wFrontCorner.y + wBackCorner.y + wLeftCorner.y + wRightCorner.y + Watersystem.GetAltitudeAt(transform.position)) * 0.2f;
            targetPosition = pos;
            pos.y = transform.position.y + (pos.y - transform.position.y)*buoyancy*Time.deltaTime;

            if (cC != null)
            {
                cC.Move(pos - transform.position);                    
            }
            else
            {
                transform.position = pos;
                    
            }

            float rollAngle;

            rollAngle = -Mathf.Atan2((wRightCorner - wLeftCorner).magnitude, wRightCorner.y - wLeftCorner.y);


            float pitchAngle = -Mathf.Atan2((wBackCorner - wFrontCorner).magnitude, wBackCorner.y - wFrontCorner.y);

            yaw = transform.rotation.eulerAngles.y;
            
            newAngles  =
                transform.rotation.eulerAngles;

            targetAngles = newAngles;
            targetAngles.x = pitchAngle * Mathf.Rad2Deg + 90;
            targetAngles.z = rollAngle * Mathf.Rad2Deg + 90;
            newAngles.x = RotateTowardsAngle(transform.eulerAngles.x, (pitchAngle * Mathf.Rad2Deg + 90), pitchSmoothing * Time.deltaTime);
            newAngles.z = RotateTowardsAngle(transform.eulerAngles.z, rollAngle * Mathf.Rad2Deg + 90,
                                                rollSmoothing*Time.deltaTime);

            transform.eulerAngles = newAngles;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y = Watersystem.GetAltitudeAt(transform.position);
            targetPosition = pos;
            pos.y = transform.position.y + (pos.y - transform.position.y) * buoyancy * Time.deltaTime;
            if (cC != null)
            {
                cC.Move(pos - transform.position);
            }
            else
            {
                transform.position = pos;

            }
        }

    }
    public float RotateTowardsAngle(float source,float target,float amount)
    {
        if (target < 0)
        {
            target += 360;
        }
        else if (target > 360)
        {
            target -= 360;
        }

        float result;
        if (source > 270 && target < 90)
        {
            result = source + ((target + 360) - source)*amount;
        }
        else if(source <90 && target > 270)
        {
            result = source - ((source + 360) - target) * amount;
            
        }
        else
        {
            result = source + (target - source)*amount;
        }
        if (result < 0)
        {
            result += 360;
        }
        else if(result > 360)
        {
            result -= 360;
        }
        return result;
    }
    void OnDrawGizmos()
    {
        if (useRotation)
        {
            /*Gizmos.color = new Color(0.97f, 0.0f, 0.5f, 0.5f);
            Gizmos.DrawSphere(wFrontCorner, 2);
        
            Gizmos.color = new Color(0.97f, 1.0f, 0.5f, 0.5f);
            Gizmos.DrawSphere(wBackCorner, 2);

            Gizmos.color = new Color(0.0f, 1.0f, 0.5f, 0.5f);
            Gizmos.DrawSphere(wLeftCorner, 2);

            Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.5f);
            Gizmos.DrawSphere(wRightCorner, 2);
            */
            Gizmos.matrix = Matrix4x4.TRS(targetPosition, Quaternion.Euler(targetAngles), Vector3.one);
            Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 0.3f);
            Gizmos.DrawCube(Vector3.zero, size);
            Gizmos.DrawWireCube(Vector3.zero, size);
        }

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.color = new Color(0.0f, 0.5f, 1.0f, 0.5f);
            Gizmos.DrawCube(Vector3.zero, size);
            Gizmos.DrawWireCube(Vector3.zero, size);
        
    }
}