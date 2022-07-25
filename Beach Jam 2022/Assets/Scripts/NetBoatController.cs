using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBoatController : EnemyController
{
    public float acceleration;
    public float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move() {
        //if attacking do not move
        if(attacking){return;}

        if (Vector3.Distance(this.transform.position, target.position) >= minDist)
        {
            //hello            
            //else go towards target position

            heading = target.position - transform.position;
            heading.Normalize();
            heading.y = 0;
            Vector3 moveVector = heading * acceleration;
            /* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
            _rb.velocity += moveVector;
            if(_rb.velocity.magnitude > maxSpeed){
                _rb.velocity.Normalize();
                _rb.velocity = _rb.velocity * maxSpeed;
            }
            //attack point will move
            attackPoint.position = _rb.transform.position + heading * 1.0f;
        }
        //move away from the player
        else{
            heading = transform.position - target.position;
            heading.Normalize();
            heading.y = 0;
            Vector3 moveVector = heading * acceleration;
            /* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
            _rb.velocity += moveVector;
            if(_rb.velocity.magnitude > maxSpeed){
                _rb.velocity.Normalize();
                _rb.velocity = _rb.velocity * maxSpeed;
            }

        }
    }
}
