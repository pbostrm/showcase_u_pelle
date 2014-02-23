using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

class TransformTools2 : Editor
{
    [MenuItem("PellePowerTools/RandomizeRotation %#g")]
    static void RandomizeRotation()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        foreach (var transform in selection)
        {
            transform.localRotation = UnityEngine.Random.rotation;
        }


    }

    [MenuItem("PellePowerTools/MoveToCenterCamera %#f")]
    static void MoveToCenterCamera()
    {
        Transform[] selection = Selection.GetTransforms(
            SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length >= 1)
        {
            Ray ray =
                Camera.current.ScreenPointToRay(new Vector3(Camera.current.GetScreenWidth()*0.5f,
                                                            Camera.current.GetScreenHeight()*0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("placing at " + hit.point);
                selection[0].transform.position = hit.point;
            }


        }

    }

    [MenuItem("PellePowerTools/CursorDuplicate %#d")]
    static void CursorDuplicate()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length >= 1)
        {
            GameObject newGameObject = (GameObject)Instantiate(selection[0].gameObject);
            newGameObject.transform.parent = selection[0].parent;
            newGameObject.transform.position = selection[0].transform.position;
            newGameObject.name = selection[0].name;


            
            Ray ray = Camera.current.ScreenPointToRay(new Vector3(Camera.current.GetScreenWidth()*0.5f,Camera.current.GetScreenHeight()*0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.Log("placing at " + hit.point);
                selection[0].transform.position = hit.point;
            }


        }


    }

    [MenuItem("PellePowerTools/CursorDuplicateScaleAndRotate %#R")]
    static void CursorDuplicateScaleAndRotate()
    {
        CursorDuplicate();
        ScaleRandomly();
        RotateAroundY();


    }
    [MenuItem("PellePowerTools/ScaleRandomly %#S")]
    static void ScaleRandomly()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        foreach (var transform in selection)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = UnityEngine.Random.Range(localScale.x * 0.8f, localScale.x * 1.2f);
            localScale.y = UnityEngine.Random.Range(localScale.y * 0.8f, localScale.y * 1.2f);
            localScale.z = UnityEngine.Random.Range(localScale.z * 0.8f, localScale.z * 1.2f);
            transform.localScale = localScale;

        }        
    }
    [MenuItem("PellePowerTools/RotateAroundY %#Y")]
    static void RotateAroundY()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        foreach (var transform in selection)
        {
            transform.RotateAroundLocal(Vector3.up,UnityEngine.Random.Range(0,360));
        }
    }

    void OnSceneGUI()
    {
        Debug.Log(Event.current.mousePosition);
    }
}