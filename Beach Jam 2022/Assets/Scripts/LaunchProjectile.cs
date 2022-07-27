using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class LaunchProjectile : MonoBehaviour
// {
//     [SerializeField] private Transform _target;
//     [SerializeField] private Transform _rotationPoint;

//     void Update()
//     {
//         Transform target = _target;
//         _rotationPoint.LookAt(target);
//     }
// }

// public class LaunchProjectile : MonoBehaviour
// {
//     [SerializeField]
//     private float FlightDurationInSeconds = 2;
//     private Spawn _currentSpawn;
//     private Camera _mainCamera;
//     private bool _isShot;

//     private void Start() {
//         _mainCamera = Camera.main;
//     }

//     public void ChangeCurrentSpawn (Spawn.NewSpawn) {
//         _currentSpawn = NewSpawn;
//         _isShot = false;
//     }

//     private void ShootWithVelocity (Vector3 TargetPosition)
//     {
//         Vector3 MovementVector = (TargetPosition - _currentSpawn.transform.position);
//         _currentSpawn.MoveWithVelocity(MovementVector / FlightDurationInSeconds);

//         _isShot = true;
//     }

//     private void Shoot()
//     {
//         RaycastHit hit;
//         Ray ray = _mainCamera ScreenPointToRay(Input.mousePosition);
//         if (Physics Raycast(ray, out hit)) 
//         {
//             ShootWithVelocity(hit point);
//         }
//     }

//     private void RotateTowardsMouse(Vector3 TargetPosition)
//     {
//         Vector3 DirectionVector = (TargetPosition - _currentSpawn.transform.position);
//         _currentSpawn.transform.forward = DirectionVector;
//     }

//     private void Shoot()
//     {
//         RaycastHit hit;
//         Ray ray = _mainCamera ScreenPointToRay(Input.mousePosition);
//         if (Physics Raycast ray, out hit) 
//         {
//             RotateTowardsMouse(hit.point);
//         }
//     }

//     void Update() {
//         if(_isShot)
//             return;
        
//         if(Input.GetMouseButtonDown(0)) {
//             RotateTowardsTarget();
//         } else if (Input.GetMouseButtonUp(0)) {
//             Shoot();
//         }
//     }
// }


public class LaunchProjectile : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    private int bulletSpeed = 50;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
    }
//     public void death() {
//         gameObject.SetActive(false);
//     }

}
