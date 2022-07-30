using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public float minRange;
    public GameObject ballPrefab;
    public float bulletSpeed;
    public bool canShoot = true;
    public float shootDelay;
    public AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = Vector3.RotateTowards(transform.forward, target.position, Mathf.PI, -Mathf.PI);
        transform.rotation = Quaternion.LookRotation(direction);
        if(Vector3.Distance(target.position, transform.position) < minRange && canShoot)
        {
            canShoot = false;
            Invoke("ShootCooldown", shootDelay);
            Shoot();
        }
    }

    public void ShootCooldown()
    {
        canShoot = true;
    }

    void Shoot()
    {
        Vector3 heading = target.position - transform.position;
        heading.Normalize();
        //Vector3 heading = mCamera.ScreenToWorldPoint(Input.mousePosition) - mCamera.ScreenToWorldPoint(player.position);
        //RaycastHit hit;
        //Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
        //if(Physics.Raycast(ray, out hit))
        //{
        //    Vector3 DirectionVector = hit.point - player.position;
        //}
        //Vector3 mousepos = mCamera.ScreenToWorldPoint(Input.mousePosition);
        var newball = Instantiate(ballPrefab, transform.position, transform.rotation);
        Vector3 direction = Vector3.RotateTowards(newball.transform.forward, target.position, 2 * Mathf.PI, 0);
        newball.transform.rotation = Quaternion.LookRotation(direction);
        newball.GetComponent<CannonBallProjectile>().target = heading;
        am.Play("CannonShot");
        //newball.GetComponent<Rigidbody>().velocity = -newball.transform.forward * bulletSpeed;
        //newball.GetComponent<InkProjectile>().target = new Vector3(heading.y*Mathf.Cos(30)*Mathf.Sin(45) - heading.x*Mathf.Sin(45), 2f, heading.y * Mathf.Cos(30) * Mathf.Sin(45) + heading.x * Mathf.Sin(45));
        //newball.GetComponent<InkProjectile>().target = new Vector3(heading.y * Mathf.Sin(45) + heading.x * Mathf.Sin(45), 0, heading.y * Mathf.Sin(45) - heading.x * Mathf.Sin(45));
        //float angle = Mathf.Rad2Deg * (Mathf.Acos(heading.x / heading.magnitude));
        //if (heading.y < 0)
        //{
        //    angle = -angle;
        //}
        //newball.transform.Rotate(0, 0, angle - 73.158f);
        //canThrow = true;
    }
}
