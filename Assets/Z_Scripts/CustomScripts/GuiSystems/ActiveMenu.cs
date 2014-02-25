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
                return;
            }
        }
        GameObject g = new GameObject("_ActiveMenu." + menuName);
        ActiveMenu am = g.AddComponent<ActiveMenu>();
        am.MenuName = menuName;
        activeMenus.Add(am);
        am.menuRect = rect;
    }
    static public void AddActiveGUIObject(string menuName, string path)
    {
        AddActiveGUIObject(menuName, path, null);
    }
    static public void AddActiveGUIObject(string menuName,string path,ActiveGUIObject.GUIButtonDelegate buttondelegate)
    {

        foreach (var activemenu in activeMenus)
        {
            if (activemenu.MenuName == menuName)
            {

                if (activemenu.mainGuiObject == null)
                {
                    activemenu.mainGuiObject = new ActiveGUIObject();
                    activemenu.mainGuiObject.Active = true;
                }
                activemenu.mainGuiObject.AddGuiObject(path,buttondelegate);
                break;
            }
        }
          
    }

    public void OnGUI()
    {

        if (mainGuiObject != null)
        {
            GUI.skin = (GUISkin)Resources.Load("ActiveGUISkin2");

            mainGuiObject.AllfatherGUIRect.x = menuRect.left;
            mainGuiObject.AllfatherGUIRect.y = menuRect.top;
            mainGuiObject.AllfatherGUIRect.height = mainGuiObject.GetNumberButtons() * (ButtonHeight + 1);
            mainGuiObject.AllfatherGUIRect.width = ((mainGuiObject.GetNumbersWidth() - 1) * 15) + ButtonWidth;

            Rect newRect = new Rect(mainGuiObject.AllfatherGUIRect);
            newRect.width += 60.0f;
            newRect.height += 60.0f;
            newRect.center = mainGuiObject.AllfatherGUIRect.center;
            //GUI.DrawTexture(BackGroundRect, (Texture)Resources.Load("whitesquare"));
            GUILayout.BeginArea(mainGuiObject.AllfatherGUIRect);
            GUILayout.BeginVertical();

            if (mainGuiObject.DrawActiveGUIObject())
            {
                //GuiHasBeenToggled = false;
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}