using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = .8f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public bool canAttack = true;
    public float coolDownSeconds;
    //coroutine to prevent attack spam

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
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        //damage hit enemies
        foreach(Collider enemy in hitEnemies){
            Debug.Log("hit");
        }
        StartCoroutine(AttackBuffer());
    }
}
