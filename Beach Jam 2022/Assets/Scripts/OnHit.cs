using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{

    public float knockbackForce;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Enemy"){
            gameObject.GetComponent<Health>().changeHealth(-other.gameObject.GetComponent<DamageOnContact>().damage);
            Vector3 dir = other.contacts[0].point - transform.position;
            dir = -dir.normalized;
            dir.y = 0;
            print(dir);
            gameObject.GetComponent<Rigidbody>().velocity = dir*knockbackForce;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
