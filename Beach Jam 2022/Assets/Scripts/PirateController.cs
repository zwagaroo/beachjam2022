using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateController : EnemyController
{
    public float acceleration;
    public float maxSpeed;
    public float coolDownSeconds;
	public float fleeRange;
	public float stopRange;
	
	private float angle;
	
	private bool foundLocation = false;
	private bool scared = false;
	
	
	private const int STATUS_WANDER = 0;
	private const int STATUS_ATTACK = 1;
	private const int STATUS_FLEE = 2;
	private const int STATUS_CIRCLE = 3;
	
	private int status = 0;
	
	private float turn = 0.0f;
 
    protected override void Rotate()
    {
        Vector3 direction = Vector3.RotateTowards(transform.forward, new Vector3(heading.x, transform.position.y, heading.z), 4 * Mathf.PI, 0);
        direction.y = 0;
        _rb.transform.rotation = Quaternion.LookRotation(direction);
    }
	
    public override void Move() {
        Rotate();
		if(!foundLocation)
		{
			angle = Random.Range(0, 6.28f);
			heading.x = 1;
			foundLocation = true;
		}
		
		
		if(status == STATUS_ATTACK)
		{
			if(!detectPlayerAttack())
			{
				status = STATUS_WANDER;
			}
			
			if(detectPlayer())
			{
				Flee();
				return;
			}
			MoveToPlayer();
			return;
		}
		else
		{
			print("SHOULD NOT BE HERE");
			heading.x = 1.1f * Mathf.Cos(angle) + 0 * Mathf.Sin(angle);
			heading.z = 0 * Mathf.Cos(angle) - 1.1f * Mathf.Sin(angle);
			if(detectPlayerAttack())
			{
				status = STATUS_ATTACK;
			}
			if(status == STATUS_WANDER)
			{
				turn = 0;
				
				if(Random.Range(0, 3000) < 20)
				{
					turn = -.01f;
					status = STATUS_CIRCLE;
					
				}
			}
			
			if(status == STATUS_CIRCLE)
			{
				turn = -.01f;
				if(Random.Range(0, 3000) < 20)
				{
					status = STATUS_WANDER;
				}
			}
			angle += turn;
		}
		
		print("imma bote");
		
		/*
		if(detectPlayer() || scared)
		{
			scared = true;
			Flee();
			return;
		}
		*/
		
		print("imma bote stil");
		
		heading.Normalize();
		heading.y = 0;
		Vector3 moveVector = heading;
		
		//This is the part that moves!
		_rb.velocity = moveVector * (maxSpeed);
		
		/*
		if(_rb.velocity.magnitude > maxSpeed){
			_rb.velocity.Normalize();
			_rb.velocity = _rb.velocity * (maxSpeed);
		}
		*/
		
		//attack point will move
		attackPoint.position = _rb.transform.position + heading * 1.0f;
    }

    private bool detectPlayerAttack(){
        Collider[] hitPlayer = Physics.OverlapSphere(_rb.position, attackRange, playerLayer);
        if(hitPlayer.Length != 0){
            return true;
        }
        return false;
    }
	
    private bool detectPlayerStop(){
        Collider[] hitPlayer = Physics.OverlapSphere(_rb.position, stopRange, playerLayer);
        if(hitPlayer.Length != 0){
            return true;
        }
        return false;
    }
	
    private bool detectPlayerOut(){
        Collider[] hitPlayer = Physics.OverlapSphere(_rb.position, attackRange*5, playerLayer);
        if(hitPlayer.Length != 0){
            return true;
        }
        return false;
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
		heading.y = 0;
		heading.Normalize();
		Vector3 moveVector = heading * acceleration;
		/* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
		_rb.velocity += moveVector;
		if(_rb.velocity.magnitude > maxSpeed*1.1f){
			_rb.velocity.Normalize();
			_rb.velocity = _rb.velocity * (maxSpeed*1.1f);
		}
		attackPoint.position = _rb.transform.position + heading * 1.0f;
	}
	
	private void MoveToPlayer()
	{
		print("EEEEK");
		heading = (transform.position - target.position) * -1;
		heading.y = 0;
		heading.Normalize();
		Vector3 moveVector = heading * acceleration;
		/* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
		
		if(!detectPlayerStop())
		{
			_rb.velocity += moveVector;
			if(_rb.velocity.magnitude > maxSpeed*1.1f){
				_rb.velocity.Normalize();
				_rb.velocity = _rb.velocity * (maxSpeed*1.1f);
			}
		}
		attackPoint.position = _rb.transform.position + heading * 1.0f;
	}
	
	private float distance(float x1, float z1, float x2, float z2)
	{
		return Mathf.Pow(Mathf.Pow(x2 - x1, 2.0f) + Mathf.Pow(z2 - z1, 2.0f), .5f);
	}
	
	private bool withinRange(float x1, float z1, float x2, float z2, float range)
	{
		return distance(x1, z1, x2, z2) < range;
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
