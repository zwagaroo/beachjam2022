using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   public float life = 3;

   void Awake()
   {
    Destroy(gameObject, life);
   }

   void OnCollisionEnter(Collision collision)
   {
    Destroy(collision.gameObject);
    Destroy(gameObject);
   }
}

// public class Bullet : MonoBehaviour
// {
//    public string target;
//    private Rigidbody rigidbody;
//    private int speed = 300;

//    void Start() {
//       rigidbody = GetComponent<Rigidbody>();
//       Vector2 direction = new Vector2(transform.up.x, transform.up.z);
//       rigidbody.velocity = direction * Time.fixedDeltaTime * speed;
//       Destroy(gameObject, 5);
//    }

//    private void OnTriggerEnter2D (Collider2D collision)
//    {
//       if (target == collision.name) {
//          Debug.Log("collision detected");
//       }
//    }

// }
