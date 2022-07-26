using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float enemyRange = 13f;
    [SerializeField] float enemyRotationSpeed = 5f;

    private Transform playerTransform;  //could be changed to 'target' 
    private EnemyBullet currentBullet;
    private float fireRate;
    private float fireRateDelta;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;   
                        //could replace this with a public function that sets target
                        //On Trigger Enter if there is multiple targets
        currentBullet = GetComponentInChildren<EnemyBullet>();
        fireRate = currentBullet.GetRateOfFire();
    }

    private void Update()
    {
        Vector3 playerGroundPos = new Vector3(playerTransform.position.x, 
                                  transform.position.y, playerTransform.position.z);

        //Check if player is not in range, then do nothing
        if(Vector3.Distance(transform.position, playerGroundPos) > enemyRange)
        {
            return; //do nothing because player is not in range
        }

        //PLAYER IN RANGE

        //Rotate Turret towards player
        Vector3 playerDirection = playerGroundPos - transform.position;
        float enemyRotationStep = enemyRotationSpeed * Time.deltaTime;
        Vector3 newLookDirection = Vector3.RotateTowards(transform.forward, playerDirection,
                                   enemyRotationStep, 0f);
        transform.rotation = Quaternion.LookRotation(newLookDirection);

        fireRateDelta -= Time.deltaTime;
        if(fireRateDelta <= 0)
        {
            currentBullet.Fire();
            fireRateDelta = fireRate;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, enemyRange); //Show a gizmo when selected
    }
}
