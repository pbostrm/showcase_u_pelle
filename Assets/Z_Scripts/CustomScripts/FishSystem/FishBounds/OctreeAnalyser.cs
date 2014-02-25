using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class OctreeAnalyser : MonoBehaviour
{

    BoundsOctree currentBound;
    public void Awake()
    {

        GL_OctreeRenderer.AddRenderDelegate(GL_Draw);
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
                GL.Color(Color.cyan);
                foreach(var neighbor in currentBound.neighbors)
                {
                    neighbor.GL_PushVertices(true);
                }
            }
            GL.Color(Color.magenta);
            currentBound.GL_PushVertices(true);
            GL.Vertex(transform.position);
            GL.Vertex(currentBound.position);
        }
    }
}
