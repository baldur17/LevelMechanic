using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    public GameObject flamePrefab;

    private float spawnInterval = 3f;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            timer = 0f;
            
            var flame = Instantiate(flamePrefab);
            flame.transform.position = gameObject.transform.position;
        }
    }
}
