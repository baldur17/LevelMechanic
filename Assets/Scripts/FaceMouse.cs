using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FaceMouse : MonoBehaviour
{
    private Camera _camera;
    private bool _isCameraNotNull;

    // Start is called before the first frame update
    void Start()
    {
        _isCameraNotNull = _camera != null;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
   