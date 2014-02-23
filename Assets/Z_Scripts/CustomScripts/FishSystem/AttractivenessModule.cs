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

    public int loopDirection = 1;

    public static readonly float[,] TetrahedronPoints = new float[4,3]
    {
        {-1.0f,-1.0f,-1.0f}, {-1.0f,1.0f,1.0f}, {1.0f,-1.0f,1.0f}, {1.0f,1.0f,-1.0f}
    };

    public float triangleSize = 0.16f;
    public Color triangleColor;
    public void Awake()
    {
        //loopDirection = UnityEngine.Random.Range(0, 1);
        if (loopDirection == 0)
        {
          //  loopDirection = -1;
        }

        //GL_Draw.AddDrawer(this);
        Attractiveness = UnityEngine.Random.Range(minAttractiveness, maxAttractiveness);
        //Attractiveness = maxAttractiveness*0.5f;
        triangleColor = hsv2rgb((1.0f/maxAttractiveness)*Attractiveness*0.66f, 1.0f, 1.0f);
        attractivenessRatio = (1/maxAttractiveness)*Attractiveness;

        if (DisableAttractiveness)
        {
            attractivenessRatio = 1.0f;
        }
        //float blue = Attractiveness - 66;
        //triangleColor = new Color((1.0f / 33.0f) * Attractiveness, (1.0f / 33.0f) * (Attractiveness - 33.0f), (1.0f / 33.0f) * (Attractiveness-66.0f));
        //DebugOutput.Shout("TriangleColor = "+triangleColor.ToString());
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
        if (FishAdministration.fishAdministration.loopingAttractiveness)
        {
            /*
            if (Attractiveness > maxAttractiveness || Attractiveness < minAttractiveness)
            {
                loopDirection = loopDirection * -1;
            }*/

            if (Attractiveness > maxAttractiveness)
            {
                Attractiveness -= (maxAttractiveness);
            }
            Attractiveness += FishAdministration.fishAdministration.attractivenessLoopSpeed*loopDirection*Time.deltaTime;
            //Attractiveness = maxAttractiveness*0.5f;
            triangleColor = hsv2rgb((1.0f / maxAttractiveness) * Attractiveness * 0.66f, 1.0f, 1.0f);
            attractivenessRatio = (1 / maxAttractiveness) * Attractiveness;
        }
    }
    public Vector3 CalculateBiasedDirection(List<FishControl> fishControls)
    {
        return Vector3.zero;
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