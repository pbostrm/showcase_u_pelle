using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class OctreeAnalyser : MonoBehaviour
{

    BoundsOctree currentBound;
    Vector3 startPosition;

    public bool ShowCaseObject;
    bool enabled;
    public void Awake()
    {
        if (ShowCaseObject)
        {
            startPosition = transform.position;

            transform.position = Vector3.one * 3000f;
        }
        


        GL_OctreeRenderer.AddRenderDelegate(GL_Draw);
    }
    public void Start()
    {
        if (ShowCaseObject)
        {
            ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree Analyzer", Hide);
            ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree Analyzer/Reset", Reset);
            ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree Analyzer/This tool shows the");
            ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree Analyzer/realtime Neighbor ");
            ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree Analyzer/generation");
        }
       
    }
    public void Hide()
    {
        enabled = !enabled;
        if (!enabled)
        {
            transform.position = Vector3.one * 3000f;
        }
        else
        {
            Reset();
        }
    }
    public void Reset()
    {
        
        transform.position = startPosition;

    }
    public void Update()
    {
        if (BoidsArea.boundsCollection != null)
        {

            currentBound = BoidsArea.boundsCollection[0].GetBound(transform.position);
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 1f);
        if (currentBound != null)
        {
            Gizmos.DrawWireCube(currentBound.position, currentBound.size * Vector3.one * 2);

            Gizmos.DrawCube(currentBound.position, currentBound.size * Vector3.one * 2);
            if (currentBound.neighbors != null)
            {
                Gizmos.color = new Color(0.3f, 1f, 1f, 0.5f);

                foreach (var neighbor in currentBound.neighbors)
                {
                    Gizmos.DrawWireCube(neighbor.position, neighbor.size * Vector3.one * 2);

                    Gizmos.DrawCube(neighbor.position, neighbor.size * Vector3.one * 2);

                }
            }
        }
    }
    public void GL_Draw()
    {
        if (currentBound != null)
        {
            

            if (currentBound.neighbors != null)
            {

                
                foreach(var neighbor in currentBound.neighbors)
                {
                    if (!neighbor.Empty)
                    {
                        GL.Color(Color.gray);

                    }
                    else
                    {
                        if (neighbor.Obstacle)
                        {
                            GL.Color(Color.magenta);

                        }
                        else
                        {
                            GL.Color(Color.cyan);
                        }
                    }
                    neighbor.GL_PushVertices(true);
                }
            }
            GL.Color(Color.green);
            currentBound.GL_PushVertices(true);
            GL.Vertex(transform.position);
            GL.Vertex(currentBound.position);
        }
    }
}
