using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class AttractivenessModule : sBehaviour
{
    public float Attractiveness;
    public float attractivenessRatio = 1.0f;
    public float maxAttractiveness = 100;
    public int minAttractiveness = 0;
    public bool toggleTriangle = true;

    public bool DisableAttractiveness;


    public static readonly float[,] TetrahedronPoints = new float[4,3]
    {
        {-1.0f,-1.0f,-1.0f}, {-1.0f,1.0f,1.0f}, {1.0f,-1.0f,1.0f}, {1.0f,1.0f,-1.0f}
    };

    public float triangleSize = 0.16f;
    public Color triangleColor;
    public void Awake()
    {
        Attractiveness = UnityEngine.Random.Range(minAttractiveness, maxAttractiveness);

        triangleColor = hsv2rgb((1.0f/maxAttractiveness)*Attractiveness*0.66f, 1.0f, 1.0f);
        attractivenessRatio = (1/maxAttractiveness)*Attractiveness;

        if (DisableAttractiveness)
        {
            attractivenessRatio = 1.0f;
        }
    }

    public Color hsv2rgb(float h, float s, float v)
    {
        h = (h%1 + 1)%1; // wrap hue

        int i = Mathf.FloorToInt(h*6);
        float f = h*6 - i;
        float p = v * (1 - s);
        float q = v * (1 - s * f);
        float t = v * (1 - s * (1 - f));


        switch (i)
        {
            case 0:
                return new Color(v,t,p);
            case 1:
                return new Color(q,v,p);

            case 2:
                return new Color(p,v,t);

            case 3:
                return new Color(p,q,v);

            case 4:
                return new Color(t,p,v);

            case 5:
                return new Color(v,p,q);
        }
        return Color.black;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            toggleTriangle = !toggleTriangle;
        }
    }

    #if UNITY_EDITOR
    public override void sDrawGizmos()
    {
        Gizmos.color = triangleColor;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(transform.position + new Vector3(TetrahedronPoints[i, 0], TetrahedronPoints[i, 1], TetrahedronPoints[i, 2])*triangleSize,transform.position);
            for (int j = i + 1; j < 4; j++)
            {
                Gizmos.DrawLine(transform.position + new Vector3(TetrahedronPoints[i, 0], TetrahedronPoints[i, 1], TetrahedronPoints[i, 2])*triangleSize,
                    transform.position + new Vector3(TetrahedronPoints[j, 0], TetrahedronPoints[j, 1], TetrahedronPoints[j, 2])*triangleSize);
            }
                
        }
        
    }
#endif

}