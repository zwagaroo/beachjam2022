using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = .5f;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer SR;
    private bool isGrounded = false;
    private bool isForwards = true;
    private bool isBackwards = false;
    private bool isRight = false;
    private bool isLeft = false;
    public bool isMoving = false;
    private Vector3 _input;
    public bool canMove = true;
    public Transform attackPoint;

    private void Update() {
        //dectect if on the ground
        DetectGrounded();
        //detect new inputs
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        //directional detection for the player
        if(!isBackwards && _input.z > 0){
            isBackwards = true;
            isForwards = false;
        } 
        else if (isBackwards && _input.z < 0) {
            isBackwards = false;
            isForwards = true;
        }

        //check if right or left only (if both right and forward then it's forward)
        if(!isRight && _input.x > 0 && _input.z == 0){
            isRight = true;
            isLeft = false;
            isBackwards = false;
        } 
        else if(!isLeft && _input.x < 0 && _input.z == 0){
            isLeft = true;
            isRight = false;
            isBackwards = false;

        } else if (_input.x != 0 && _input.z != 0){
            isRight = false;
            isLeft = false;
        }
        else if(_input.x == 0 && _input.z != 0){
            isRight = false;
            isLeft = false;
        }
        if(!isMoving && _input.magnitude != 0){
            isMoving = true;
        } else if (isMoving && _input.magnitude == 0){
            isMoving = false;
        }

        if(isMoving && isRight && !isLeft && !isBackwards){
            SR.flipX = true;
        } else{
            SR.flipX =false;
        }
        anim.SetBool("isBackwards", isBackwards);
        anim.SetBool("isRight", isRight);
        anim.SetBool("isLeft", isLeft);
        anim.SetBool("isMoving", isMoving);

        //set where the attackpoint is located
        if(isRight){
            attackPoint.position = gameObject.transform.position + new Vector3(1.2f, 0,-1.2f);
        }
        else if(isLeft){
            attackPoint.position = gameObject.transform.position + new Vector3(-1.2f, 0, 1.2f);
        }
        else if(isBackwards){
            attackPoint.position = gameObject.transform.position + new Vector3(1.2f, 0, 1.2f);
        }
        else{
            attackPoint.position = gameObject.transform.position + new Vector3(-1.2f, 0, -1.2f);
        }

        //detect jumping
        if(Input.GetButtonDown("Jump") && isGrounded){
            _rb.velocity += new Vector3(0f,jumpForce, 0f);     
        }

        if(gameObject.GetComponent<Health>().isInvincible) {
            canMove = false;
        }
        else {
            canMove = true;
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
        if(!canMove){ return;}
        var moveVector = _input.ToIso();
        moveVector.Normalize();
        moveVector *= _speed;
        moveVector.y = _rb.velocity.y*Time.deltaTime;
        _rb.transform.position += moveVector;
        
    }

    public virtual IEnumerator speedChange(float amount, float seconds){
        _speed *= amount;
        yield return new WaitForSeconds(seconds);
        _speed /= amount;

    }
}

