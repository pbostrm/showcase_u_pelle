using System;
using UnityEngine;
using System.Collections.Generic;

class GL_OctreeRenderer : MonoBehaviour
{

	private static Material lineMaterial;

    public bool RenderEnabled = true;
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
    public void Start()
    {
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree render",ToggleGLRender,enabled);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree render/Dynamic Obstacle", ToggleObstacle, RenderObstacle);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree render/Render Empty", ToggleEmpty, RenderEmpty);
        ActiveMenu.AddActiveGUIObject("LeftSideMenu", "Octree render/Static Obstacle", ToggleS_Obstacle, RenderObstacle);

    }
    public void ToggleGLRender()
    {
        RenderEnabled = !RenderEnabled;
    }
    public void ToggleObstacle()
    {
        RenderObstacle = !RenderObstacle;
    }
    public void ToggleEmpty()
    {
        RenderEmpty = !RenderEmpty;
    }
    public void ToggleS_Obstacle()
    {
        RenderNonEmpty = !RenderNonEmpty;
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


        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        if (RenderEnabled)
        {
            boundsOctree.GL_Draw(false, RenderEmpty, false);
            boundsOctree.GL_Draw(false, false, RenderNonEmpty);
            boundsOctree.GL_Draw(RenderObstacle, false, false);
        }


        if (renderDelegates != null) //should actually make a proper GL_render component that deals with this.
        {
            foreach (var renderDelagate in renderDelegates)
            {
                renderDelagate();
            }
        }
        	
        GL.End();
		
	}
}
