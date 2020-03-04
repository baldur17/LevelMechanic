using System;
using System.Collections;
using System.Collections.Generic;
using AGDDPlatformer;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    public GameObject flamePrefab;

    public float spawnInterval = 3f;
    public float startTimer = 0f;

    private float _timer;
    // Start is called before the first frame update
    void Start()
    {
        _timer = startTimer;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > spawnInterval)
        {
            _timer = 0f;
            
            var flame = Instantiate(flamePrefab);
            flame.transform.position = gameObject.transform.position + new Vector3(0, -0.2f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            GameManager.instance.PlayerDeath();
        }
    }
}
