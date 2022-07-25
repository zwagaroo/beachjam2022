using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
    {
    //target    
    [SerializeField] public Transform target;
    [SerializeField] public Rigidbody _rb;
    [SerializeField] public float _speed = 5;
    //have to be this far away at least
    [SerializeField] public float minDist;
    public bool farAway;

    //Point of attack
    public Transform attackPoint;
    //Range of attack
    public float attackRange;

    //Damage of attack
    public float attackDamage;

    //Attacks players so it's attacks affect the player layer
    public LayerMask playerLayer;

    //Speed of attack in seconds (pause motion on attacks)
    public float attackSpeedSeconds;
    
    //force of knockback of attacks
    public float knockbackForce;
    
    //[SerializeField] private float _turnSpeed = 360;

    //direction where enemy is heading
    public Vector3 heading; 

    //attacking or not
    public bool attacking;

    public bool canAttack = true;
    
    void Update()
    {
        if(detectPlayer()){
            //start attack routine
            if(!attacking){
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    void FixedUpdate()
    {
        //movement controls
            Move();
    }

    //private void Follow()
    //{
    //    _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    //}

    //private void Look()
    //{
    //    if (_input == Vector3.zero) return;

    //    var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    //}


    //can be overriden
    public virtual void Move()
    {
        if (Vector3.Distance(this.transform.position, target.position) > minDist)
        {
            //if attacking do not move
            if(attacking){return;}
            //else go towards target position
            heading = target.position - transform.position;
            heading.Normalize();
            heading.y = 0;
            Vector3 moveVector = heading * _speed * Time.deltaTime;
            /* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
            _rb.transform.position += moveVector;
            //attack point will move
            attackPoint.position = _rb.transform.position + heading * 1.0f;
        }
    }

    //player detection can be overriden
    public virtual bool detectPlayer(){
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        if(hitPlayer.Length != 0){
            return true;
        }
        return false;
    }

    //attack routine can be overriden
    public virtual IEnumerator AttackCoroutine(){
        if(!canAttack){ yield break; }
        attacking = true;
        yield return new WaitForSeconds(attackSpeedSeconds);
        Attack();
        attacking = false;
    }

    //attacking can be overriden
    public virtual void Attack(){
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        if(hitPlayer.Length != 0){ 
            foreach(Collider player in hitPlayer){
                player.gameObject.GetComponent<Health>().changeHealth(-1*attackDamage);
                Vector3 knockbackDir = player.gameObject.transform.position - transform.position;
                knockbackDir = knockbackDir.normalized;
                knockbackDir.y = 0;
                player.gameObject.GetComponent<Rigidbody>().velocity = knockbackDir*knockbackForce;
            }
        }
    }

}
