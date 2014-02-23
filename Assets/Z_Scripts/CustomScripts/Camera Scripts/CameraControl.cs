using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public MouseLook.RotationAxes axes = MouseLook.RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;


    public void Awake()
    {


    }
    void Update()
    {
        if (axes == MouseLook.RotationAxes.MouseXAndY)
        {
            if (Input.GetMouseButton(1))
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
                rotationY = transform.localEulerAngles.x + Input.GetAxis("Mouse Y")*-sensitivityY;
                
                //rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                //rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                if (rotationY <= 360 + minimumY && rotationY >= 180)
                {
                    rotationY = 360 + minimumY;
                }
                else if (rotationY > maximumX && rotationY < 180)
                {
                    rotationY = minimumY;
                }
                transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
                
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                Input.ResetInputAxes();
            }
        }
        else if (axes == MouseLook.RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
        transform.position += transform.forward*Input.GetAxis("Mouse ScrollWheel")*30;
        transform.position += transform.right * Input.GetAxis("Horizontal")*Time.deltaTime*100;
        transform.position += transform.forward * Input.GetAxis("Vertical")*Time.deltaTime*100;
    }

}
