using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FishAdministration : MonoBehaviour
{
    static FishAdministration _fishAdministration;
    public static FishAdministration fishAdministration
    {
        get
        {
            if (_fishAdministration == null)
            {
                GameObject gamesystem = GameObject.Find("_SceneSystem");
                if (gamesystem == null)
                {
                    gamesystem = new GameObject("_SceneSystem");
                }
                _fishAdministration = gamesystem.AddComponent<FishAdministration>();

            }
            return _fishAdministration;
        }
    }


    public bool EnableAttractiveness;
    public static bool _enAttractiveness
    {
        get
        {
            if (_fishAdministration == null)
            {

                return false;
            }
            return _fishAdministration.EnableAttractiveness;
        }
    }

    public bool loopingAttractiveness = false;
    public float attractivenessLoopSpeed = 1.0f;
    public void Awake()
    {
        _fishAdministration = this;
    }
}