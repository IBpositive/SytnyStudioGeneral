using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    private Camera _mainCamera;
    private Ray ray;
    private RaycastHit hitInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = _mainCamera.transform.position;
        ray.direction = _mainCamera.transform.forward;
        Physics.Raycast(ray, out hitInfo);
        transform.position = hitInfo.point;
    }
}
