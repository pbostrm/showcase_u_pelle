using UnityEngine;
using System.Collections;

public class GL_HandleLine : MonoBehaviour 
{
    public Color color;
	// Use this for initialization
	void Awake () 
    {
        GL_OctreeRenderer.AddRenderDelegate(GL_Draw);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void GL_Draw()
    {
        if (transform.parent != null)
        {
            GL.Color(color);
            GL.Vertex(transform.position);
            GL.Vertex(transform.parent.position);
        }

    }
}
