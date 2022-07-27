using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkProjectile : MonoBehaviour
{
    public Vector3 target;
    public int moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 heading = target - transform.position;
        heading.Normalize();
        heading.y = 0;
        transform.position += heading * moveSpeed * Time.deltaTime;
    }
}
