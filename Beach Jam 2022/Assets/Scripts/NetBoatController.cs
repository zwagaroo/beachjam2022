using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBoatController : EnemyController
{
    public float acceleration;
    public float maxSpeed;
    public float coolDownSeconds;
 
    public override void Move() {
        //if attacking do not move
        if(attacking){return;}

        if (Vector3.Distance(this.transform.position, target.position) >= minDist)
        {        
            //else go towards target position

            heading = target.position - transform.position;
            heading.Normalize();
            heading.y = 0;
            Vector3 moveVector = heading * acceleration;
            /* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
            _rb.AddForce(moveVector, ForceMode.VelocityChange);
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

    private IEnumerator AttackBuffer()
    {
        Debug.Log(this.name +"Cannot attack");
        canAttack = false;

        yield return new WaitForSeconds(coolDownSeconds);

        canAttack = true;
        Debug.Log(this.name +"can attack");
    
    }

    public override void Attack(){
        print("running");
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        if(hitPlayer.Length != 0){ 
            foreach(Collider player in hitPlayer){
                print(player.name);
                player.gameObject.GetComponent<Health>().changeHealth(-1*attackDamage);
                StartCoroutine(player.gameObject.GetComponent<PlayerController>().speedChange(.2f, 3));
            }
        }
        StartCoroutine(AttackBuffer());
    }


}
