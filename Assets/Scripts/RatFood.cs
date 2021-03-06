﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatFood : MonoBehaviour {

    public bool eaten = false;
    public float regenTime = 10.0f;
    public float lastEaten = 0.0f;
    Renderer foodRenderer;

    private void Start()
    {
        foodRenderer = GetComponent<Renderer>();
    }

    public void Eat()
    {
        eaten = true;
        foodRenderer.enabled = false;
        lastEaten = Time.time;
    }

    public void Update()
    {
        if (Time.time > (lastEaten + regenTime))
        {
            eaten = false;
            foodRenderer.enabled = true;
        }
    }

}
