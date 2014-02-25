using System;
using UnityEngine;
using System.Collections.Generic;

class GL_OctreeRenderer : MonoBehaviour
{

	private static Material lineMaterial;

    public bool enabled = true;
    public bool RenderObstacle = true;
    public bool RenderEmpty = true;
    public bool RenderNonEmpty = true;
	public BoundsOctree boundsOctree;

    public delegate void GL_OctreeRenderDelegate();

    static public List<GL_OctreeRenderDelegate> renderDelegates;
    public static void CreateLineMaterial() 
	{
		if( !lineMaterial ) {
			lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
			                            "SubShader { Pass { " +
			                            "    Blend SrcAlpha OneMinusSrcAlpha " +
			                            "    ZWrite Off Cull Off Fog { Mode Off } " +
			                            "    BindChannels {" +
			                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
			                            "} } }" );
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}
	public static GL_OctreeRenderer OctreeRenderer;

	public void Awake()
	{
		OctreeRenderer = this;
		CreateLineMaterial();
		
	}
    public static void AddRenderDelegate(GL_OctreeRenderDelegate func)
    {
        if (renderDelegates == null)
        {

            renderDelegates = new List<GL_OctreeRenderDelegate>();
        }
        if (!renderDelegates.Contains(func))
        {
            renderDelegates.Add(func);
        }
    }
	public void OnPostRender() {


        if (enabled)
        {
            lineMaterial.SetPass(0);
            GL.Begin(GL.LINES);

            boundsOctree.GL_Draw(false,RenderEmpty,false);
            boundsOctree.GL_Draw(false,false,RenderNonEmpty);
            boundsOctree.GL_Draw(RenderObstacle,false,false);

            if (renderDelegates != null)
            {
                foreach (var renderDelagate in renderDelegates)
                {
                    renderDelagate();
                }
            }
            GL.End();
        }	
		
	}
    public void OnGUI()
    {
        if (GUI.Button(new Rect(10, 40, 200, 18), "GL: "+enabled.ToString()))
        {
            enabled = !enabled;
        }
        if (enabled)
        {
            if (GUI.Button(new Rect(10, 62, 200, 18), "Obstacle Render: " + RenderObstacle.ToString()))
            {
                RenderObstacle = !RenderObstacle;
            }
            if (GUI.Button(new Rect(10, 84, 200, 18), "Empty Render: " + RenderEmpty.ToString()))
            {
                RenderEmpty = !RenderEmpty;
            }
            if (GUI.Button(new Rect(10, 108, 200, 18), "Environment Render: " + RenderNonEmpty.ToString()))
            {
                RenderNonEmpty = !RenderNonEmpty;
            }
        }
        GUI.Label(new Rect(10f, Screen.height - 40f, 200f, 30f), "Pelle Bostrom 2014");
       
    }
}
