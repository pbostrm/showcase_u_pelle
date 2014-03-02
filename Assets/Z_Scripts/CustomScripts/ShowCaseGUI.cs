using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ShowCaseGUI :MonoBehaviour
{
    public void Awake()
    {
        ActiveMenu.CreateActiveMenu("LeftSideMenu", new Rect(40, 100, 16, 120));

        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Obstacle Ghost", BoidsArea.ToggleLazyGhost);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Obstacle Ghost/is On", null, true);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "ObstacleAvoidance", ToggleObstacleAvoidance);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "ObstacleAvoidance/PewPew", null);

    }
    public void ToggleObstacleAvoidance()
    {
        ObstacleAvoidance.toggleDebugObstacle = !ObstacleAvoidance.toggleDebugObstacle;
    }
}
