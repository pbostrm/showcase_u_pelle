using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Watersystem : MonoBehaviour
{
    Renderer _playerWaves;
    Renderer playerWaves
    {
        get
        {
            if (_playerWaves == null)
            {
                _playerWaves = Waves.PlayerWave;
            }
            return _playerWaves;
        }
    }

    Renderer _optionWaves;
    Renderer optionWaves
    {
        get
        {
            if (_optionWaves == null)
            {
                _optionWaves = Waves.OptionWave;
            }
            return _optionWaves;
        }
    }
    static public Watersystem ws;
    static public Watersystem _Watersystem
    {
        get
        {
            if (ws == null)
            {
                GameObject gamesystem = GameObject.Find("Gamesystem");
                if (gamesystem == null)
                {
                    gamesystem = new GameObject("Gamesystem");
                }
                ws = gamesystem.AddComponent<Watersystem>();

            }
            return ws;
        }
    }

    WaveGerstnerData PlayerWaveData;
    WaveGerstnerData OptionWaveData;

    static public bool IsCameraUnderWater()
    {
        try
        {
			//return UnderWaterEffect.CamUnderWater;
        }
        catch
        {
            if (Camera.main.transform.position.y < GetAltitudeAt(Camera.main.transform))
            {
                return true;
            }
        }
        return false;
    }

    public void Awake()
    {
        PlayerWaveData = new WaveGerstnerData(1);
        OptionWaveData = new WaveGerstnerData(1);
    }

    void Update()
    {
        
        if (optionWaves != null)
        {
            OptionWaveData.ImportData(optionWaves.sharedMaterial);
            OptionWaveData.externalAltitude = optionWaves.transform.position.y;

        }
        if (playerWaves != null)
        {
            PlayerWaveData.ImportData(playerWaves.sharedMaterial);
            PlayerWaveData.externalAltitude = playerWaves.transform.position.y;
        }
        else if(optionWaves !=null)
        {
            PlayerWaveData = OptionWaveData;
        }

    }
    static float GetAltitudeAt(Transform t)
    {
        return GetAltitudeAt(t.position);
    }
    static public float GetAltitudeAt(Vector3 position)
    {
        //check if out of range
        if (_Watersystem == null)
        {
            DebugOutput.Shout("We are out of luck!");
                return 0;
        }
        return _Watersystem.PlayerWaveData.GetAltitudeAt(position);

    }

}
public struct WaveGerstnerData
{
    public float gerstnerIntensity;

    public float externalAltitude;

    public Vector4 waveAmp;
    public Vector4 waveFreq;
    public Vector4 waveSteep;
    public Vector4 waveSpeed;
    public Vector4 waveDirAB;
    public Vector4 waveDirCD;

    public WaveGerstnerData(int b)
    {
        gerstnerIntensity = 1.0f;
        externalAltitude = 0.0f;
        waveAmp = Vector4.zero;
        waveFreq = Vector4.zero;
        waveSteep = Vector4.zero;
        waveSpeed = Vector4.zero;
        waveDirAB = Vector4.zero;
        waveDirCD = Vector4.zero;
    }
    public void ImportData(Material mat)
    {
        waveAmp = mat.GetVector("_GAmplitude");
        waveFreq = mat.GetVector("_GFrequency");
        waveSteep = mat.GetVector("_GSteepness");
        waveSpeed = mat.GetVector("_GSpeed");
        waveDirAB = mat.GetVector("_GDirectionAB");
        waveDirCD = mat.GetVector("_GDirectionCD");
    }
    
    public float GetAltitudeAt(Vector3 pos)
    {

        Vector2 xzVertex = new Vector2(pos.x,pos.z);
        
        Vector4 AB = new Vector4(waveSteep.x * waveAmp.x * waveDirAB.x,
                                 waveSteep.x * waveAmp.x * waveDirAB.y,
                                 waveSteep.y * waveAmp.y * waveDirAB.z,
                                 waveSteep.y * waveAmp.y * waveDirAB.w);

        Vector4 CD = new Vector4(waveSteep.z * waveAmp.z * waveDirAB.x,
                                 waveSteep.z * waveAmp.z * waveDirAB.y,
                                 waveSteep.w * waveAmp.w * waveDirAB.z,
                                 waveSteep.w * waveAmp.w * waveDirAB.w);

        Vector4 dotX = new Vector4(
            Vector2.Dot(new Vector2(waveDirAB.x, waveDirAB.y), xzVertex),
            Vector2.Dot(new Vector2(waveDirAB.z, waveDirAB.w), xzVertex),
            Vector2.Dot(new Vector2(waveDirCD.x, waveDirCD.y), xzVertex),
            Vector2.Dot(new Vector2(waveDirCD.z, waveDirCD.w), xzVertex));

        Vector4 dotABCD = new Vector4(waveFreq.x * dotX.x,
                                      waveFreq.y * dotX.y,
                                      waveFreq.z * dotX.z,
                                      waveFreq.w * dotX.w);

        float time = Time.time;
        Vector4 TIME = new Vector4(time * waveSpeed.x, time * waveSpeed.y, time * waveSpeed.z, time * waveSpeed.w);


        Vector4 COS = new Vector4(Mathf.Cos(dotABCD.x + TIME.x),
                                  Mathf.Cos(dotABCD.y + TIME.y),
                                  Mathf.Cos(dotABCD.z + TIME.z),
                                  Mathf.Cos(dotABCD.w + TIME.w));

        Vector4 SIN = new Vector4(Mathf.Sin(dotABCD.x + TIME.x),
                                  Mathf.Sin(dotABCD.y + TIME.y),
                                  Mathf.Sin(dotABCD.z + TIME.z),
                                  Mathf.Sin(dotABCD.w + TIME.w));

        Vector3 offset = new Vector3();
        offset.x = Vector4.Dot(COS, new Vector4(AB.x, AB.z, CD.x, CD.z));
        offset.z = Vector4.Dot(COS, new Vector4(AB.y, AB.w, CD.y, CD.w));
        offset.y = Vector4.Dot(SIN, waveAmp);

        return offset.y+externalAltitude;
    }
}
