using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ActiveMenu : MonoBehaviour
{
    public static List<ActiveMenu> activeMenus;
    public Rect menuRect;
    public string MenuName;

    public ActiveGUIObject mainGuiObject;

    public float ButtonHeight = 16.0f;
    public float ButtonWidth = 128.0f;

    public static void CreateActiveMenu(string menuName, Rect rect)
    {
        if (activeMenus == null)
        {
            activeMenus = new List<ActiveMenu>();
        }
        
        foreach (var activemenu in activeMenus)
        {
            if (activemenu.MenuName == menuName)
            {
                activemenu.menuRect = rect;
                return;
            }
        }
        GameObject g = new GameObject("_ActiveMenu_" + menuName);
        ActiveMenu am = g.AddComponent<ActiveMenu>();
        am.MenuName = menuName;
        am.menuRect = rect;

        activeMenus.Add(am);
    }
    static public void AddActiveGUIObject(string menuName, string path)
    {
        AddActiveGUIObject(menuName, path, null,false);
    }
    static public void AddActiveGUIObject(string menuName,string path,ActiveGUIObject.GUIButtonDelegate bDelegate)
    {
        AddActiveGUIObject(menuName, path, bDelegate, false);
    }
    static public void AddActiveGUIObject(string menuName,string path,ActiveGUIObject.GUIButtonDelegate bDelegate,bool ActivatedOnStart)
    {
        if (activeMenus == null)
        {
            CreateActiveMenu(menuName, new Rect(10, 10, 10, 10));
        }
        foreach (var activemenu in activeMenus)
        {
            if (activemenu.MenuName == menuName)
            {

                if (activemenu.mainGuiObject == null)
                {
                    activemenu.mainGuiObject = new ActiveGUIObject();
                    activemenu.mainGuiObject.Active = true;
                }
                activemenu.mainGuiObject.AddGuiObject(path, bDelegate,ActivatedOnStart);
                break;
            }
        }
          
    }

    public void OnGUI()
    {

        if (mainGuiObject != null)
        {
            GUI.skin = (GUISkin)Resources.Load("ActiveGUISkin2");

            mainGuiObject.AllfatherGUIRect.x = menuRect.xMin;
            mainGuiObject.AllfatherGUIRect.y = menuRect.yMin;
            mainGuiObject.AllfatherGUIRect.height = mainGuiObject.GetNumberButtons() * (ButtonHeight + 1);
            mainGuiObject.AllfatherGUIRect.width = ((mainGuiObject.GetNumbersWidth() - 1) * 15) + ButtonWidth;

            Rect newRect = new Rect(mainGuiObject.AllfatherGUIRect);
            newRect.width += 60.0f;
            newRect.height += 60.0f;
            newRect.center = mainGuiObject.AllfatherGUIRect.center;
            //GUI.DrawTexture(BackGroundRect, (Texture)Resources.Load("whitesquare"));
            GUILayout.BeginArea(mainGuiObject.AllfatherGUIRect);
            GUILayout.BeginVertical();

            mainGuiObject.DrawActiveGUIObject();


            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}