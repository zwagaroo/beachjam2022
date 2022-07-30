using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoatController : EnemyController
{
    public float acceleration;
    public float maxSpeed;
    public float coolDownSeconds;
	public float approachRange;
	public bool approachPlayer;
	
	private bool foundDirection = false;
	private bool scared = false;
	private int timeLeft;
	private int accelMultiplier;
	private float angle;
	
    public override void Move() {
        Rotate();
	
		heading.x = 1.1f * Mathf.Cos(angle) + 0 * Mathf.Sin(angle);
		heading.z = 0 * Mathf.Cos(angle) - 1.1f * Mathf.Sin(angle);
		angle += -.01f;
		
		heading.Normalize();
		heading.y = 0;
		Vector3 moveVector = heading * 0;
		/* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
		_rb.velocity += moveVector;
		if(_rb.velocity.magnitude > maxSpeed){
			_rb.velocity.Normalize();
			_rb.velocity = _rb.velocity * (maxSpeed);
		}
		//attack point will move
		attackPoint.position = _rb.transform.position + heading * 1.0f;
    }
	
    private IEnumerator AttackBuffer()
    {
        Debug.Log(this.name +"Cannot attack");
        canAttack = false;

        yield return new WaitForSeconds(coolDownSeconds);

        canAttack = true;
        Debug.Log(this.name +"can attack");
    
    }


}
