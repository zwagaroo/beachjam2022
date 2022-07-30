using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Animator[] tentacleAnims;
    public float attackRange = .8f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public float knockbackForce;

    public bool canAttack = true;
    public float attackDamage;
    public float coolDownSeconds;
    public AudioManager am;
    public AudioSource aSource;
    public AudioClip slap;
    //coroutine to prevent attack spam

    void Start(){
        am = FindObjectOfType<AudioManager>();
    }

    private IEnumerator AttackBuffer()
    {
        Debug.Log("Cannot attack");
        canAttack = false;

        yield return new WaitForSeconds(coolDownSeconds);
        canAttack = true;
        Debug.Log("Player can attack");
    
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Attack();
        }
    }

    void Attack(){
        if(!canAttack){return;}

        //Collider[] hitEnemies = attackPoint.GetComponent<MarkForAttack>().inRangeEnemies.ToArray();
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        //damage hit enemies
        foreach (Animator ani in tentacleAnims){
            ani.SetTrigger("isAttacking");
        }
        anim.SetTrigger("isAttacking");
        print(am.sounds);
        am.Play("Player_Slap");
        foreach(Collider enemy in hitEnemies){
            if (enemy != null)
            {
                var enemyHealth = enemy.gameObject.GetComponent<Health>();
                enemyHealth.changeHealth(-attackDamage);
                if(enemyHealth.isDead())
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
}
