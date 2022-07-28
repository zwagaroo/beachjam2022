using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    public Transform target;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float minDist;
    private bool farAway;
    public Transform attackPoint;
    public float attackRange;
    public float attackDamage;
    public LayerMask playerLayer;
    public float attackSpeedSeconds;
    public float knockbackForce;
    
    //[SerializeField] private float _turnSpeed = 360;

    private Vector3 heading; 
    private bool attacking;


    void Update()
    {
        //if(detectPlayer()){
        //    StartCoroutine(AttackCoroutine());
        //}
    }
    private void FixedUpdate()
    {
        //Follow();
        //Look();
        if (Vector3.Distance(this.transform.position, target.position) > minDist)
        {            
            Move();
        }
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


    private void Rotate()
    {
        Vector3 direction = Vector3.RotateTowards(transform.forward, new Vector3(heading.x, transform.position.y, heading.z), 4 * Mathf.PI, 0);
        direction.y = 0;
        _rb.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Move()
    {
        if(attacking){return;}
        heading = target.position - transform.position;
        heading.Normalize();
        heading.y = 0;
        Vector3 moveVector = heading * _speed * Time.deltaTime;
        Rotate();
        /* var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime); */
        _rb.transform.position += moveVector;
        attackPoint.position = _rb.transform.position + heading * 1.0f;
    }

    private bool detectPlayer(){
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        if(hitPlayer.Length != 0){
            return true;
        }
        return false;
    }

    private IEnumerator AttackCoroutine(){
        attacking = true;
        yield return new WaitForSeconds(attackSpeedSeconds);
        Attack();
        attacking = false;
    }

    private void Attack(){
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
