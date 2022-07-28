using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailboatController : EnemyController
{
    public float acceleration;
    public float maxSpeed;
    public float coolDownSeconds;
	public float fleeRange;
	
	private bool foundDirection = false;
	private bool scared = false;
 
    public override void Move() {
        Rotate();
		if(!foundDirection)
		{
			heading.x = Random.Range(-10, 10);
			heading.z = Random.Range(-10, 10);
			foundDirection = true;
		}
		
		print("imma bote");
		
		if(detectPlayer() || scared)
		{
			scared = true;
			Flee();
			return;
		}
		
		print("imma bote stil");
		
		heading.Normalize();
		heading.y = 0;
		Vector3 moveVector = heading * acceleration;
		/* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
		_rb.velocity += moveVector;
		if(_rb.velocity.magnitude > maxSpeed){
			_rb.velocity.Normalize();
			_rb.velocity = _rb.velocity * (maxSpeed);
		}
		//attack point will move
		attackPoint.position = _rb.transform.position + heading * 1.0f;
    }

    //player detection can be overriden
    public override bool detectPlayer(){
        Collider[] hitPlayer = Physics.OverlapSphere(_rb.position, fleeRange, playerLayer);
        if(hitPlayer.Length != 0){
            return true;
        }
        return false;
    }
	
	private void Flee()
	{
		print("EEEEK");
		heading = transform.position - target.position;
		heading.Normalize();
		heading.y = 0;
		Vector3 moveVector = heading * acceleration;
		/* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
		_rb.velocity += moveVector;
		if(_rb.velocity.magnitude > maxSpeed*1.1f){
			_rb.velocity.Normalize();
			_rb.velocity = _rb.velocity * (maxSpeed*1.1f);
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
                player.gameObject.GetComponent<Health>().changeHealth(-1*attackDamage);
                StartCoroutine(player.gameObject.GetComponent<PlayerController>().speedChange(.5f, 3));
            }
        }
        StartCoroutine(AttackBuffer());
    }


}
