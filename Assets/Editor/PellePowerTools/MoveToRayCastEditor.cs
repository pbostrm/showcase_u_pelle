using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveToRayCast))]
class MoveToRayCastEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Debug.Log("BÄÖÖÖÖÖ");

        Debug.Log(Event.current.mousePosition);

        Ray ray = Camera.current.ScreenPointToRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000.0f))
        {
            Debug.Log(hit.point);
            Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

            if (selection.Length >= 1)
            {
                selection[0].transform.position = hit.point;
                
            }        
        }
        Debug.Log("BUUUUUU");


    }
}
