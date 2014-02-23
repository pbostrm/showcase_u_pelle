using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class FishColorChanger : MonoBehaviour
{
    public Renderer fishRenderer;
    AttractivenessModule attractivenessModule;

    public void Start()
    {
        fishRenderer.sharedMaterial = new UnityEngine.Material(fishRenderer.sharedMaterial);
        attractivenessModule = gameObject.GetComponent<AttractivenessModule>();
        fishRenderer.sharedMaterial.color =attractivenessModule.triangleColor;
    }
    public void Update()
    {
        fishRenderer.sharedMaterial.color = attractivenessModule.triangleColor;

    }
}
