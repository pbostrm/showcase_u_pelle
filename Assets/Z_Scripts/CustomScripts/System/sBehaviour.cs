using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


public class sBehaviour : MonoBehaviour
{
    public bool isEditorSelect;
    public Color32 mainGizmoColor;
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Transform[] selection = Selection.GetTransforms(
        SelectionMode.TopLevel | SelectionMode.Editable);

        isEditorSelect = selection.Contains(transform);
        if (isEditorSelect)
        {
            Gizmos.color = mainGizmoColor;
            sDrawGizmos();            
        }
    }
    public virtual void sDrawGizmos()
    {
        
    }
#endif
}

