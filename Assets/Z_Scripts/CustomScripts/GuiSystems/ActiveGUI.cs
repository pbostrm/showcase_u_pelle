using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ActiveGUI : MonoBehaviour
{
    static public bool ActiveGUIToggled;
    static public float ButtonHeight = 16.0f;
    static public float ButtonWidth = 128.0f;
    static public Vector3 mouseAnchorpos;
    ActiveGUIObject mainGuiObject;
    public bool GuiHasBeenToggled;
    bool GUIupdatedThisFrame;

    
    static ActiveGUI _activeGUI;
    public static ActiveGUI activeGUI
    {
        get
        {
            if (_activeGUI == null)
            {
                GameObject g = new GameObject("_ActiveGuiObject");
                _activeGUI = g.AddComponent<ActiveGUI>();
            }
            return _activeGUI;
        }
    }
    public void checkClearActiveGui()
    {

    }
    public void AddActiveGUIObject(string path)
    {
        if(mainGuiObject == null)
        {
            mainGuiObject = new ActiveGUIObject();
            mainGuiObject.Active = true;
        }
        if(mainGuiObject !=null)
        {
            mainGuiObject.AddGuiObject(path);
        }
    }
    public void Update()
    {
        if (GuiHasBeenToggled)
        {
            ActiveGUIToggled = false;
            GuiHasBeenToggled = false;
            mainGuiObject.DeactivateChilds();
        }
        
        

        if (Input.GetMouseButtonDown(1))
        {
            //DebugOutput.Shout("Toggling on ActiveGUI " + ActiveGUIToggled);

            mouseAnchorpos = Input.mousePosition;

            if (!ActiveGUIToggled)
            {
                ActiveGUIToggled = true;
                //mainGuiObject.DeactivateChilds();
            }
            else
            {
                mainGuiObject.DeactivateChilds();
                //GuiHasBeenToggled = true;
                //ActiveGUIToggled = false;
            }

        }
        if(!GuiHasBeenToggled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //GuiHasBeenToggled = true;
            }
        }
        if (ActiveGUIToggled)
        {
            mainGuiObject.AllfatherGUIRect.x = mouseAnchorpos.x;
            mainGuiObject.AllfatherGUIRect.y = Screen.height - mouseAnchorpos.y;
            mainGuiObject.AllfatherGUIRect.height = mainGuiObject.GetNumberButtons() * (ButtonHeight+1);
            mainGuiObject.AllfatherGUIRect.width = ((mainGuiObject.GetNumbersWidth()-1) * 15) + ButtonWidth;

            Rect newRect = new Rect(mainGuiObject.AllfatherGUIRect);
            newRect.width += 60.0f;
            newRect.height += 60.0f;
            newRect.center = mainGuiObject.AllfatherGUIRect.center;

            if (!newRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y )))
            {
                GuiHasBeenToggled = true;
            }
        }


    }
    public void Awake()
    {
        if (_activeGUI != null)
        {
            Destroy(this);
        }
        ActiveGUIToggled = false;
    }
    public void OnGUI()
    {


        if (ActiveGUIToggled)
        {
            if (mainGuiObject != null && mouseAnchorpos != null)
            {
                GUI.skin = (GUISkin)Resources.Load("ActiveGUISkin2");

                mainGuiObject.AllfatherGUIRect.x = mouseAnchorpos.x;
                mainGuiObject.AllfatherGUIRect.y = Screen.height-mouseAnchorpos.y;
                mainGuiObject.AllfatherGUIRect.height = mainGuiObject.GetNumberButtons() * (ButtonHeight+1);
                mainGuiObject.AllfatherGUIRect.width = ((mainGuiObject.GetNumbersWidth()-1) * 15)+ButtonWidth;

                mainGuiObject.AllfatherGUIRect = PixelPerfect.Perfect(mainGuiObject.AllfatherGUIRect);
                Rect BackGroundRect = new Rect(mainGuiObject.AllfatherGUIRect);
                BackGroundRect.width += 4;
                BackGroundRect.height += 4;
                BackGroundRect.center = mainGuiObject.AllfatherGUIRect.center;
                //GUI.DrawTexture(BackGroundRect, (Texture)Resources.Load("whitesquare"));
                GUILayout.BeginArea(mainGuiObject.AllfatherGUIRect);
                GUILayout.BeginVertical();
                
                if (mainGuiObject.DrawActiveGUIObject())
                {
                    GuiHasBeenToggled = false;
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        
    }

}
class ActiveGUIObject
{
    public delegate void GUIButtonDelegate();

    public GUIButtonDelegate buttonDelegate;


    public ActiveGUIObject AllfatherGuiObject;
    List<ActiveGUIObject> ActiveGuiObjects;
    public Rect AllfatherGUIRect;

    
    public bool Active;

    public string name;
    public ActiveGUIObject()
    {
        AllfatherGUIRect = new Rect(0, 0, 500, 1000);
    }
    public ActiveGUIObject(string n)
    {
        name = n;
    }
    public ActiveGUIObject(string n,ActiveGUIObject fatherObject)
    {
        name = n;
        AllfatherGuiObject = fatherObject;

    }
    public int GetNumbersWidth()
    {
        if (ActiveGuiObjects == null)
        {
            return 1;
        }
        else
        {
            int a = 1;
            int maxA = 0;
            if (Active)
            {
                foreach (var ago in ActiveGuiObjects)
                {
                    if (ago.GetNumbersWidth() > maxA)
                    {
                        maxA = ago.GetNumbersWidth();
                    }
                }
            }

            if (name == null)
            {
                a -= 1;
            }
            return a+maxA;
        }
        
    }
    public int GetNumberButtons()
    {
        int a = 1;
        if (Active)
        {
            if (ActiveGuiObjects != null)
            {
                foreach (var ago in ActiveGuiObjects)
                {
                   a += ago.GetNumberButtons();
                }
            }
            
        }
        if (name == null)
        {
            a = a -1;
        }
        return a;
    }

    public void AddGuiObject(string path)
    {
        AddGuiObject(path,null,false );
    }
    public void AddGuiObject(string path,GUIButtonDelegate bdelegate,bool ActivatedOnStart)
    {
        if(ActiveGuiObjects == null)
        {
            ActiveGuiObjects = new List<ActiveGUIObject>();
        }
        if(ActiveGuiObjects !=null)
        {
            Active = ActivatedOnStart;

            if (path.Contains("/"))
            {
                string folder = path.Substring(0, path.IndexOf('/'));
                string cutoff = path.Substring(path.IndexOf('/') + 1);

                foreach (var AGO in ActiveGuiObjects)
                {
                    if (AGO.name.ToLowerInvariant() == folder.ToLowerInvariant())
                    {
                        AGO.AddGuiObject(cutoff, bdelegate,ActivatedOnStart);
                        return;
                    }
                }

                

                if(AllfatherGuiObject==null){AllfatherGuiObject = this;}
                ActiveGUIObject newAGO = new ActiveGUIObject(folder, AllfatherGuiObject);
                newAGO.AddGuiObject(cutoff, bdelegate, ActivatedOnStart);
                ActiveGuiObjects.Add(newAGO);
            }
            else
            {
                foreach (var AGO in ActiveGuiObjects)
                {
                    if (AGO.name.ToLowerInvariant() == path.ToLowerInvariant())
                    {
                        if (bdelegate != null)
                        {
                            buttonDelegate = bdelegate;
                        }
                        return;
                    }
                }
                if (AllfatherGuiObject == null) { AllfatherGuiObject = this; }
                ActiveGUIObject newAGO = new ActiveGUIObject(path, AllfatherGuiObject);
                newAGO.buttonDelegate = bdelegate;
                newAGO.Active = ActivatedOnStart;
                ActiveGuiObjects.Add(newAGO);
            }
        }
    }

    public bool DrawActiveGUIObject()
    {
        bool guiHasBeenUsed = false;

        if (name != null)
        {
            GUIStyle ButtonStyle;
            if (ActiveGuiObjects != null)
            {
                ButtonStyle = GUI.skin.GetStyle("button");
            }
            else
            {
                ButtonStyle = GUI.skin.GetStyle("ButtonOption1");
            }
            
            if (GUILayout.Button(name,ButtonStyle, GUILayout.Width(ActiveGUI.ButtonWidth), GUILayout.Height(ActiveGUI.ButtonHeight)))
            {

                    if (buttonDelegate != null)
                    {
                        buttonDelegate();
                    }
                    Active = !Active;
                    if(ActiveGuiObjects != null && ActiveGuiObjects.Count >0)
                    {
                        if(Active)
                        {
                            //add height to base group.
                        }
                    }
                    guiHasBeenUsed = true;

            }
        }
        if (Active)
        {
            if (ActiveGuiObjects != null)
            {
                if (ActiveGuiObjects.Count >= 1)
                {
                    if (name != null)
                    {
                        GUI.BeginGroup(new Rect(15, 0, Screen.width, Screen.height));
                    }
                    foreach (var ago in ActiveGuiObjects)
                    {
                        if (ago.DrawActiveGUIObject())
                        {
                            guiHasBeenUsed = true;
                        }
                    }

                    if (name != null)
                    {
                        GUI.EndGroup();
                    }
                }
            }
        }
        return guiHasBeenUsed;
        
    }
    public void DeactivateChilds()
    {
        if (ActiveGuiObjects != null)
        {
            foreach (var ago in ActiveGuiObjects)
            {
                if (ago.Active)
                {
                    ago.DeactivateChilds();
                    ago.Active = false;
                }
            }
        }
        
        //Active = false;
    }
}