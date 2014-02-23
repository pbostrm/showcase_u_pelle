using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ShipController : MonoBehaviour
{

    public static List<ShipController> controllableShips = new List<ShipController>(); 
    public float yawAcc = 1.0f;
    public float yawSpeed = 0.0f;
    public float topYawSpeed = 10.0f;

    public float forwardAcc = 1.0f;
    public float forwardSpeed;
    public float topForwardSpeed =10.0f;

    public bool acceleratingForward = false;
    public bool acceleratingBackward = false;
    public Transform CameraAnchor;
    public bool Controllable;
    public Transform playerAnchorPoint;
    public GameObject AvatarModel;
    public GameObject Player;
    //public GameObject dummy;
    void Awake()
    {
        controllableShips.Add(this);
        //Debug.Log("--------------------------------------------");
        foreach (var controllableShip in controllableShips)
        {
            //Debug.Log(controllableShip);
        }
    }
    void Update()
    {
        if (!Controllable)
        {
            if (AvatarModel != null)
            {
                AvatarModel.SetActive(false);
            }
            /*foreach (var componentsInChild in dummy.GetComponentsInChildren<Renderer>())
            {
                componentsInChild.renderer.enabled = false;
            }*/
            return;
            
        }
        else
        {
            if (AvatarModel != null)
            {
                AvatarModel.SetActive(true);
            }
            /*foreach (var componentsInChild in dummy.GetComponentsInChildren<Renderer>())
            {
                componentsInChild.renderer.enabled = true;
            }*/
        }
        //Yaw
        if (Input.GetKey(KeyCode.A))
        {
            if (yawSpeed < topYawSpeed)
            {
                yawSpeed += yawAcc * Time.deltaTime;         
            }
        }
        else
        {
            if(yawSpeed > 0.0f)
            {
                yawSpeed -= Mathf.Min((yawAcc * Time.deltaTime),yawSpeed);
            }
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (yawSpeed > -topYawSpeed)
            {
                yawSpeed -= yawAcc * Time.deltaTime;
            }
        }
        else
        {
            if (yawSpeed < 0.0f)
            {
                yawSpeed += Mathf.Max((yawAcc * Time.deltaTime), yawSpeed);
            }
        }
        transform.Rotate(Vector3.up,-yawSpeed*Time.deltaTime);

        //speed
        if (Input.GetKey(KeyCode.W))
        {
            if (forwardSpeed < topForwardSpeed)
            {
                forwardSpeed += forwardAcc * Time.deltaTime;
                acceleratingForward = true;
            }
            
        }
        else
        {
            if (forwardSpeed > 0.0f)
            {
                forwardSpeed -= Mathf.Min((forwardAcc * Time.deltaTime), forwardSpeed);
                acceleratingForward = false;

            }

        }
        if (Input.GetKey(KeyCode.S))
        {
            if (forwardSpeed > -topForwardSpeed)
            {
                forwardSpeed -= forwardAcc * Time.deltaTime;
                acceleratingBackward = true;
            }

        }
        else
        {
            if (forwardSpeed < 0.0f)
            {
                acceleratingBackward = false;
                forwardSpeed += Mathf.Max((forwardAcc * Time.deltaTime), forwardSpeed);
            }
        }

        CharacterController cC = GetComponent<CharacterController>();
        cC.Move(transform.forward*forwardSpeed*Time.deltaTime);
        //transform.position += transform.forward*forwardSpeed*Time.deltaTime;
    }
}
