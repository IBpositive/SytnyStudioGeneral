using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/channel/UCjszZMwnOW4fO5VIDU_Wh1Q
public class CharacterLocomotion : MonoBehaviour
{
    private Animator _animator;
    private Vector2 _input;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Controller();
    }

    private void Controller()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        _animator.SetFloat("InputX", _input.x);
        _animator.SetFloat("InputY", _input.y);
    }
}
