using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetProjectile : CannonBallProjectile
{
    public bool onPlayer = false;

    //so the target can be tracked exactly;
    public Transform absoluteTarget;

    IEnumerator FollowPlayer()
    {
        onPlayer = true;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onPlayer = true;
            transform.SetParent(absoluteTarget);
            transform.position = absoluteTarget.position;
            StartCoroutine(FollowPlayer());
        }
    }

    public override void Update()
    {
        if (!onPlayer)
        {
            Vector3 heading = absoluteTarget.position - transform.position;
            heading.Normalize();
            transform.position += heading * moveSpeed * Time.deltaTime;
        }
    }
}
