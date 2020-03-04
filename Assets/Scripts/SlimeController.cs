using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using AGDDPlatformer;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    private float _timer;
    private float _spinInterval;
    private bool _isSpinning;
    private float _spinTime;
    private float _spinTimeCounter;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        _spinInterval = 1.6f;
        _isSpinning = false;
        _animator = gameObject.GetComponent<Animator>();
        _spinTime = 1.183f;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        Debug.Log(_timer);
        if (_isSpinning)
        {
            //Count if Slime is spinning
            _spinTimeCounter += Time.deltaTime;
            _timer = 0f;
        }
        
        if (_timer > _spinInterval)
        {
            _animator.Play("SlimeSpin");
            _isSpinning = true;
            _spinTimeCounter = 0f;
        }

        if (_spinTimeCounter >= _spinTime)
        {
            _animator.Play("SlimeIdle");
            _timer = 0f;
            _isSpinning = false;
            _spinTimeCounter = 0f;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool hittingPlayer = other.CompareTag("Player1");

        //Slime is colliding with player
        if (hittingPlayer)
        {
            if (GameManager.instance.players[0].getIsDashing())
            {
                //Player is dashing currently

                if (_isSpinning)
                {
                    GameManager.instance.PlayerDeath();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                GameManager.instance.PlayerDeath();
            }
        }
    }
}
