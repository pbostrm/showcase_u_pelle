using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FishNicknameRenderer : MonoBehaviour
{
    public bool left;
    public void OnGUI()
    {
        foreach (var fc in FishControl.fishCollection)
        {
            Vector3 screenpoint = camera.WorldToScreenPoint(fc.transform.position);

            if ((fc.transform.position-transform.position).magnitude < 45)
            {
                if (left)
                {
                    if (screenpoint.x < Screen.width * 0.5f)
                    {
                        GUI.Label(new Rect(screenpoint.x, Screen.height - screenpoint.y, 50, 50), fc.Nickname);

                    }

                }
                else if (screenpoint.x > Screen.width * 0.5f)
                {
                    GUI.Label(new Rect(screenpoint.x, Screen.height - screenpoint.y, 50, 50), fc.Nickname);

                }
            }

            
            
        }
    }

}
 