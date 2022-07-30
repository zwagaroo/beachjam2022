using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBoatController : EnemyController
{
    public float acceleration;
    public float maxSpeed;
    public float coolDownSeconds;
    public GameObject net;
    
    //what degree the net hurts player movement
    public float speedDecrease = .5f;

    //how long net lasts
    public float netTime = 3f;

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
        
        if (hitPlayer.Length != 0){
            ThrowNet(target.position);
            foreach(Collider player in hitPlayer){
                player.gameObject.GetComponent<Health>().changeHealth(-1*attackDamage);
                StartCoroutine(player.gameObject.GetComponent<PlayerController>().speedChange(speedDecrease, netTime));                
            }
        }
        StartCoroutine(AttackBuffer());
    }

    void ThrowNet(Vector3 position)
    {
        target = LevelManager.Instance.player.transform;
        Vector3 heading = target.position - transform.position;
        heading.Normalize();
        var newnet = Instantiate(net, transform.position, transform.rotation);
        //Vector3 direction = Vector3.RotateTowards(newnet.transform.forward, target.position, 2 * Mathf.PI, 0);
        //newnet.transform.rotation = Quaternion.LookRotation(direction);
        newnet.GetComponent<NetProjectile>().target = heading;
        newnet.GetComponent<NetProjectile>().absoluteTarget = target;
    }

}
