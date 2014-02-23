using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//>> Marcus Add
using System.IO;
//<<

using UnityEngine;

class DebugOutput : MonoBehaviour
{
    public static List<string> outputs;
    private GUIStyle outputLinesStyle;
    public static string outputText = "";
    public static bool newText;
    public GUISkin defaultSkin;
    public bool toggleConsole = true;

    void Awake()
    {
        outputs = new List<string>();

        outputLinesStyle = GUIStyle.none;
        outputLinesStyle.padding = new RectOffset(1, 1, -10, 0);
    }

    void Update()
    {
        if (outputs.Count >= 50)
        {
            outputs.RemoveAt(0);
        }
        if (Input.GetKeyDown("'"))
        {
            toggleConsole = !toggleConsole;

        }
    }

    void OnGUI()
    {
        if (newText)
        {
            outputText = "";
            //for (int i = outputs.Count-1; i >= 0; i--)
            //{
            //    outputText = outputText + outputs[i] + "\n";
            //}

            for (int i = 0; i < outputs.Count; i++)
                outputText = outputText + outputs[i] + "\n";

        }
        if (toggleConsole)
        {
            if (defaultSkin == null)
                GUI.TextArea(new Rect(0, 0, Screen.width, (Screen.height / 2)), outputText);
            else
                GUI.TextArea(new Rect(0, 0, Screen.width, (Screen.height / 2)), outputText, defaultSkin.textArea);


        }
    }

    static public void Shout(string text)
    {
        newText = true;
        outputs.Add(sTime.timeStamp + ": " + text);
        Debug.Log(text);
    }

    static public void PrintLogFile(String filename, bool timestamp, params string[] strings)
    {
        string t = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString() + System.DateTime.Today.Day.ToString();

        string completeName = (timestamp ? filename + "_" + t + ".txt" : filename + ".txt");

        if (!System.IO.Directory.Exists("Logs/"))
            System.IO.Directory.CreateDirectory("Logs");

        using (System.IO.StreamWriter file = new StreamWriter("Logs/" + completeName, true))
        {

            file.WriteLine(System.DateTime.Now.ToString("HH:mm:ss"));
            foreach (string s in strings)
            {
                file.WriteLine(s);
                Shout(s);

            }
            file.WriteLine("");
        }
    }
}
