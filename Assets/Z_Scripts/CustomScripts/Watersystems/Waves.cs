using UnityEngine;
using System.Collections;

public class Waves : MonoBehaviour
{
    public bool isPlayerWave;
    public static Renderer PlayerWave;
    public static Renderer OptionWave;

    public static Renderer TargetWaves; //Deprecated 
    public static Renderer primaryWave; //Deprecated 
	// Use this for initialization
	void Awake ()
	{
        if (isPlayerWave)
        {
            PlayerWave = renderer;
        }
        else
        {
            OptionWave = renderer;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
