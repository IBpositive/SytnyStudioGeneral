using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


//https://www.youtube.com/channel/UCjszZMwnOW4fO5VIDU_Wh1Q
public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f;
    [SerializeField] private float aimDuration = 0.8f;
    private Camera _mainCamera;
    [SerializeField] private Rig _aimLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = _mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0,yawCamera,0), turnSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
       // if (Input.GetButton("Fire2"))
       // {
       //     _aimLayer.weight += Time.deltaTime / aimDuration;
       // }
       // else
       // {
       //     _aimLayer.weight -= Time.deltaTime / aimDuration;
       // }

       _aimLayer.weight = 1f;
    }
}
