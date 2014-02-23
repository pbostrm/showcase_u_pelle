using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GL_CircleRenderer : GL_Object
{
    public Vector3 AroundAxle;
    public int Resolution = 100;
    public float Radius = 2.0f;
    float x
    {
        get { return transform.position.x; }
    }
    float y
    {
        get { return transform.position.y; }
    }
    float z
    {
        get { return transform.position.z; }
    }
    public Color color;
    public void Awake()
    {
        GL_LineRenderer.GL_Objects.Add(this);
    }
    public override void GL_Draw()
    {
        base.GL_Draw();
        GL.Color(color);
        Vector3 startPoint = transform.position + new Vector3(Radius, 0, 0);
        Vector3 LinePoint = startPoint;
        Vector3 lastLinePoint;
        for (int i = 0; i < Resolution; i++)
        {
            lastLinePoint = LinePoint;
            LinePoint = RotateAroundPoint(startPoint, transform.position,
                                              Quaternion.AngleAxis((360 / Resolution)*i, AroundAxle));
            GL.Vertex3(lastLinePoint.x, lastLinePoint.y, lastLinePoint.y);

            //GL.Vertex3(LinePoint.x, LinePoint.y, LinePoint.y);
            GL.Vertex3(LinePoint.x, LinePoint.y, LinePoint.y);
            //GL.Vertex3(LinePoint);
            //GL.Vertex3(x, y, z);
            if (i == 2)
            {
                //Debug.Log("e " + lastLinePoint+" "+ LinePoint);
            }
        }
  

        
        //GL.Vertex3(x+Radius, y, z);
        

    }
    static Vector3 RotateAroundPoint( Vector3 point, Vector3 pivot, Quaternion angle) 
    {
        return angle * ( point - pivot) + pivot;
        /*Vector3 finalPos = point - pivot;
        //Center the point around the origin
        finalPos = angle * finalPos;
        //Rotate the point.
 
        finalPos += pivot;
        //Move the point back to its original offset. 
 
        return finalPos;*/
    }
}
