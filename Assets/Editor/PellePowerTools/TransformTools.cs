using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class TransformTools : Editor
{
    [MenuItem("PellePowerTools/Copy into Torus")]
    static void CopyIntoTorus()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);
        if (selection.Length == 1)
        {
            foreach (var transform in selection)
            {
                SceneView.lastActiveSceneView.pivot = transform.position;
            }
        }
    }
    [MenuItem("PellePowerTools/SnapToPosition")]
    static void SnapToPosition()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length > 0)
        {
            foreach (var transform in selection)
            {
                transform.position = selection[0].position;
            }
        }
    }
    [MenuItem("PellePowerTools/axis/SnapToX")]
    static void SnapToX()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length > 0)
        {
            foreach (var transform in selection)
            {
                Vector3 pos = transform.position;
                pos.x = selection[0].position.x;
                transform.position = pos;
            }
        }
    }
    [MenuItem("PellePowerTools/axis/SnapToY")]
    static void SnapToY()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length > 0)
        {
            foreach (var transform in selection)
            {
                Vector3 pos = transform.position;
                pos.y = selection[0].position.y;
                transform.position = pos;
            }
        }
    }
    [MenuItem("PellePowerTools/axis/SnapToZ")]
    static void SnapToZ()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length > 0)
        {
            foreach (var transform in selection)
            {
                Vector3 pos = transform.position;
                pos.z = selection[0].position.z;
                transform.position = pos;
            }
        }
    }
    [MenuItem("PellePowerTools/Move Selection to Camera Pivot")]
    static void MoveToCameraPivot()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length > 0)
        {
            foreach (var transform in selection)
            {
                transform.position = SceneView.lastActiveSceneView.pivot;
            }
        }
    }
    [MenuItem("PellePowerTools/Make Selection Pivot")]
    static void MakeSelectionPivot()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length == 1)
        {
            foreach (var transform in selection)
            {
                SceneView.lastActiveSceneView.pivot = transform.position;
            }
        }
    }

    [MenuItem("PellePowerTools/SnapToTerrain")]
    static void SnapToTerrain()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        LayerMask layerMask;
        layerMask = LayerMask.NameToLayer("Terrain");

        if (layerMask != null)
        {
            if (selection.Length > 0)
            {
                foreach (var transform in selection)
                {
                    Ray ray = new Ray(transform.position,Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10000.0f))
                    {
                        transform.position = hit.point;
                    }
                    else
                    {
                        ray.direction = Vector3.up;
                        if (Physics.Raycast(ray, out hit, 10000.0f))
                        {
                            transform.position = hit.point;
                        }
                        else
                        {
                            Debug.Log(ray.direction);
                        }  
                    }
                        
                    
                }
            }   
        }
        

    }

    [MenuItem("CONTEXT/GameObject/Create Parent For Transforms")]
    static void CreateParentForTransformsRightClick(MenuCommand command)
    {
        Debug.Log("SHIT!");

    }
    [MenuItem("PellePowerTools/Create Parent For Transforms")]
    static void CreateParentForTransforms()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        if (selection.Length > 0)
        {
            GameObject selectionParent = new GameObject(selection[0].name+"s");
            foreach (var transform in selection)
            {
                transform.parent = selectionParent.transform;
            }
        }

    }
    [MenuItem("PellePowerTools/axis/SnapToX", true)]
    [MenuItem("PellePowerTools/axis/SnapToY", true)]
    [MenuItem("PellePowerTools/axis/SnapToZ", true)]
    [MenuItem("PellePowerTools/Move Selection to Camera Pivot", true)]
    [MenuItem("PellePowerTools/Make Selection Pivot", true)]
    [MenuItem("PellePowerTools/SnapToPosition", true)]
    [MenuItem("PellePowerTools/Create Parent For Transforms", true)]
    static bool ValidateSelection()
    {
        return Selection.activeGameObject != null;
    }
}

