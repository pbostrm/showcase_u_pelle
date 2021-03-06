﻿using UnityEngine;
using System.Collections;

public class AnimationSpeed : MonoBehaviour
{
    public float speed = 1.0f;
	public void Awake()
	{
		speed = Random.Range(1.0f,4.0f);
	}
    void Update()
    {
        foreach (AnimationState state in animation)
        {
            state.speed = speed;
        }
    }
}