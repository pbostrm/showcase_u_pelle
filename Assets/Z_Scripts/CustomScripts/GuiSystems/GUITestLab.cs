using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GUITestLab : MonoBehaviour
{
    public void Start()
    {
        ActiveMenu.CreateActiveMenu("LeftSideMenu", new Rect(300,100,16,120));
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Test123");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Testabc",DoStuff);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Testabc/lalal/banan/kaka");
       /* ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Testabc/lolol",DoStuff);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Testabc/lolol/traffle/lol");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Testabc/lolol");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","Testing/Testabc/lolol");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","LOLOL/Test123");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","apfar/Test123");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","LaunchTheMissile!/Test123");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","LaunchTheMissile!/kalle");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","LaunchTheMissile!/balle");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","LaunchTheMissile!/fikon");
        ActiveMenu.AddActiveGUIObject("LeftSideMenu","LaunchTheMissile!/skalle");
        */
        /*ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Test123");
        ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Testabc");
        ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Testabc/lalal/banan/kaka");
        ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Testabc/lolol");
        ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Testabc/lolol/traffle/lol");
        ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Testabc/lolol");
        ActiveGUI.activeGUI.AddActiveGUIObject("Testing/Testabc/lolol");
        ActiveGUI.activeGUI.AddActiveGUIObject("LOLOL/Test123");
        ActiveGUI.activeGUI.AddActiveGUIObject("apfar/Test123");
        ActiveGUI.activeGUI.AddActiveGUIObject("LaunchTheMissile!/Test123");
        ActiveGUI.activeGUI.AddActiveGUIObject("LaunchTheMissile!/kalle");
        ActiveGUI.activeGUI.AddActiveGUIObject("LaunchTheMissile!/balle");
        ActiveGUI.activeGUI.AddActiveGUIObject("LaunchTheMissile!/fikon");
        ActiveGUI.activeGUI.AddActiveGUIObject("LaunchTheMissile!/skalle");*/
    }

    public void DoStuff()
    {
        DebugOutput.Shout("DOING STUFF!!");
    }
}
