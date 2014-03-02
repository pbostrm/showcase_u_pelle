using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class MagicTools : Editor
{
    [MenuItem("PellePowerTools/Magic Wand/the magicRuler %#d")]
    static void CursorDuplicate()
    {
        GameObject rulerObject = GameObject.Find("!Rulerobjects");
        if (rulerObject == null)
        {
            rulerObject = new GameObject("!Rulerobjects");
            rulerObject.AddComponent<MagicWandRuler>();
        }

        Ray ray = Camera.current.ScreenPointToRay(new Vector3(Camera.current.GetScreenWidth() * 0.5f, Camera.current.GetScreenHeight() * 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Debug.Log("Created A Magic Ruler at" + hit.point+ " depth: "+hit.point.y);
            
            rulerObject.transform.position = hit.point;
        }

    }
}
