using System.Collections.Generic;
using UnityEngine;
using System.Collections;

class GL_LineRenderer : MonoBehaviour
{
    private static Material lineMaterial;
    public static List<GL_Object> GL_Objects = new List<GL_Object>();
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
    public void Awake()
    {

    }
    public void OnPostRender() {
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass( 0 );
        GL.Begin(GL.LINES);
        for (int i = GL_Objects.Count - 1; i >= 0; i--)
        {
            if (GL_Objects[i] == null)
            {
                GL_Objects.RemoveAt(i);
                continue;
            }
            GL_Objects[i].GL_Draw();

        }

        /*GL.Vertex3(0, 0, 0);
        GL.Vertex3(1, 0, 0);
        GL.Vertex3(0, 1, 0);
        GL.Vertex3(1, 1, 0);
        GL.Color( new Color(0,0,0,0.5f) );
        GL.Vertex3( 0, 0, 0 );
        GL.Vertex3( 0, 1, 0 );
        GL.Vertex3( 1, 0, 0 );
        GL.Vertex3( 1, 1, 0 );*/
        GL.End();

        
    }
}

public struct GL_Line 
{
    public GL_Line(Vector3 s,Vector3 e)
    {
        start = s;
        end = e;
    }
    public Vector3 start;
    public Vector3 end;
}
