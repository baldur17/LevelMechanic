using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlameController : MonoBehaviour
{
    private const float Speed = 5;
    public bool left = true;

    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        if (left)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.left * Speed;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * Speed;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //SceneManager.LoadScene(2);
    }
}
