﻿using System;
using System.Collections;
using System.Collections.Generic;
using AGDDPlatformer;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player1"))
        {
            GameManager.instance.PlayerDeath();
        }
    }
}
