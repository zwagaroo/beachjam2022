using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkProjectile : MonoBehaviour
{
    public Vector3 target;
    public Vector3 rotation;
    public int moveSpeed;
    public float dmg = 5;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 direction = Vector3.RotateTowards(transform.up, target, 7f, -7f);
        //target.y = 0;
        //transform.rotation = Quaternion.LookRotation(direction);
        //float angle = Mathf.Rad2Deg*(Mathf.Asin(target.z/target.magnitude));
        //transform.Rotate(0, 0, angle - 90);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heading = target;
        heading.y = 0;
        transform.position += heading * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            var enemyHealth = other.GetComponent<Health>();
            other.GetComponent<Health>().changeHealth(-dmg);
            if (enemyHealth.isDead())
            {
                enemyHealth.gameObject.GetComponent<ExplosionDeath>().Death();
                LevelManager.levelManager.enemies.RemoveAt(0);
                Debug.Log("enemy removed");
                if (LevelManager.levelManager.enemies.Count == 0)
                {
                    LevelManager.levelManager.NextLevel();
                }
            }
            Destroy(gameObject);
        }
    }
}
