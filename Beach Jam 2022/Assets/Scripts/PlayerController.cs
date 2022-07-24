using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = .5f;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;
    private bool isGrounded = false;
    private Vector3 _input;


    private void Update() {
        DetectGrounded();
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if(Input.GetButtonDown("Jump") && isGrounded){
            _rb.velocity += new Vector3(0f,jumpForce, 0f);     
        }
    }

    private void DetectGrounded(){
        RaycastHit hit;
        if(Physics.Raycast(groundPoint.position, Vector3.down, out hit, .3f, groundMask)){
            isGrounded = true;
        }else{
            isGrounded = false;
        }
    }

    private void FixedUpdate() {
        Move();
    }


    private void Move() {
        var moveVector = _input.ToIso();
        moveVector.Normalize();
        moveVector *= _speed;
        moveVector.y = _rb.velocity.y*Time.deltaTime;
        _rb.transform.position += moveVector;

    }
}

