using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class sTime : MonoBehaviour
{
    public float TimeModifier = 1.0f;
    static public float deltaTime
    {
        get { return Time.deltaTime*_sTime.TimeModifier; }
    }
    static public string timeStamp
    {
        get
        {
            return System.DateTime.Now.ToString();            
        }
    }
    public static sTime _sTime;
    
    public void Awake()
    {
        _sTime = this;
    }
}