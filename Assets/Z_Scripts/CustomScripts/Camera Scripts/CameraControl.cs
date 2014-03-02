using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    Vector3 startPos;

    public void Awake()
    {
        startPos = transform.position;

    }
    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            rotationY = transform.localEulerAngles.x + Input.GetAxis("Mouse Y")*-sensitivityY;
                

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

        transform.position += transform.forward*Input.GetAxis("Mouse ScrollWheel")*30;
        transform.position += transform.right * Input.GetAxis("Horizontal")*Time.deltaTime*100;
        transform.position += transform.forward * Input.GetAxis("Vertical")*Time.deltaTime*100;
    }

    public void OnGUI()
    {
        if ((transform.position - startPos).sqrMagnitude < 100)
        {
            GUI.contentColor = Color.blue;
            GUI.Label(new Rect(Screen.width * 0.35f, Screen.height * 0.5f, 400, 20), "Use WASD to move and Right mousebutton to rotate");
    
        }
     }

}
