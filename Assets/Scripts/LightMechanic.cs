using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightMechanic : MonoBehaviour
{
    public Light pointLight;
    public Light flashLight;
    public Light spotLight;
    
    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(ReduceLightSource), 0.2f, 0.2f);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void ReduceLightSource()
    {
        if (flashLight.range <= 0)
        {
            //Restart level
        }

        var range = pointLight.range;
        range -= 0.7f;
        pointLight.range = range;
        flashLight.range = range;
        if (range < 10f)
        {
            spotLight.range -= 1.4f;
        }
    }
}
