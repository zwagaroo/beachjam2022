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
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heading = target;
        //Vector3 direction = Vector3.RotateTowards(transform.up, target, 7f, -7f);
       
        heading.y = 0;
        transform.position += heading * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Health>().changeHealth(-dmg);
        }
    }
}
