using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class PlayerController : MonoBehaviour {
    public bool inputDisabled = false;

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

    public AudioSource audioSource;
    public AudioClip[] swimSounds;

    public AudioManager am;

    //Stuff from playerAttack
    [SerializeField] private Animator[] tentacleAnims;
    public float attackRange = .8f;
    public LayerMask enemyLayer;
    public float knockbackForce;

    public bool canAttack = true;
    public float attackDamage;
    public float coolDownSeconds;

    public AudioClip slap;
    //End of stuff from playerAttack


    void Start(){
        audioSource = GetComponent<AudioSource>();
        am = FindObjectOfType<AudioManager>();
    }

    private void Update() {
        //dectect if on the ground
        DetectGrounded();


        if(!inputDisabled){
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

            if(isRight && !isBackwards){
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
            //From playerAttack
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }

        if(gameObject.GetComponent<Health>().isInvincible) {
            canMove = false;
        }
        else {
            canMove = true;
        }
        
        swimSoundsPlay();
 
        
    }

    void swimSoundsPlay(){
               //SWIM SOUNDS
/*         Array.Find(am.sounds, sound => sound.name == "swim1").source.Play(); */

        bool swim1Playing = Array.Find(am.sounds, sound => sound.name == "swim1").source.isPlaying;
        bool swim2Playing = Array.Find(am.sounds, sound => sound.name == "swim2").source.isPlaying;
        bool swim3Playing = Array.Find(am.sounds, sound => sound.name == "swim3").source.isPlaying;
        bool swim4Playing = Array.Find(am.sounds, sound => sound.name == "swim4").source.isPlaying;
        bool swim5Playing = Array.Find(am.sounds, sound => sound.name == "swim5").source.isPlaying;
        bool swim6Playing = Array.Find(am.sounds, sound => sound.name == "swim6").source.isPlaying;
        bool allNotPlaying = !swim1Playing && !swim2Playing && !swim3Playing && !swim4Playing && !swim5Playing && !swim6Playing;
        //print(allNotPlaying);
        if (allNotPlaying && isMoving)
        {
            int rand = UnityEngine.Random.Range(1, 5);
            if(rand == 1){
                am.Play("swim1");
            }
            else if(rand == 2){
                am.Play("swim2");
            }
            else if(rand == 3){
                am.Play("swim3");
            }
            else if(rand == 4){
                am.Play("swim4");
            }
            else if(rand == 5){
                am.Play("swim6");
            }
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
        if(!inputDisabled){
            Move();
        }
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


    //From PlayerAttack
    private IEnumerator AttackBuffer()
    {
        Debug.Log("Cannot attack");
        canAttack = false;

        yield return new WaitForSeconds(coolDownSeconds);
        canAttack = true;
        Debug.Log("Player can attack");

    }

    //From PlayerAttack
    void Attack()
    {
        if (!canAttack) { return; }

        //Collider[] hitEnemies = attackPoint.GetComponent<MarkForAttack>().inRangeEnemies.ToArray();
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        //damage hit enemies
        foreach (Animator ani in tentacleAnims)
        {
            ani.SetTrigger("isAttacking");
        }
        anim.SetTrigger("isAttacking");
        am.Play("Player_Slap");
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy != null)
            {
                var enemyHealth = enemy.gameObject.GetComponent<Health>();
                enemyHealth.changeHealth(-attackDamage);
                if (enemyHealth.isDead())
                {
                    enemy.gameObject.GetComponent<ExplosionDeath>().Death();
                    LevelManager.Instance.enemies.RemoveAt(0);
                    Debug.Log("enemy removed");
                    if (LevelManager.Instance.enemies.Count == 0)
                    {
                        LevelManager.Instance.FinishLevel();
                    }
                }
                Vector3 knockbackDir = enemy.gameObject.transform.position - attackPoint.position;
                knockbackDir = knockbackDir.normalized;
                knockbackDir.y = 0;
                enemy.gameObject.GetComponent<Rigidbody>().velocity = knockbackDir * knockbackForce;
            }
        }
        StartCoroutine(AttackBuffer());
    }

    void OnTriggerEnter(Collider col){ //When player leaves map, create new level
        Debug.Log("PLAYER COLLIDED WITH " + col.gameObject.name);
        if(col.gameObject.tag == "LevelManager")
        {
            col.gameObject.GetComponent<LevelManager>().TransitionToNewLevel(col);
        }
    }
}

