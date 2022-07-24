using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float minDist;
    private bool farAway;
    //[SerializeField] private float _turnSpeed = 360;

    void Update()
    {
    }
    private void FixedUpdate()
    {
        //Follow();
        //Look();
        if (Vector3.Distance(this.transform.position, target.position) > minDist)
        {
            Move();
        }
    }

    //private void Follow()
    //{
    //    _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    //}

    //private void Look()
    //{
    //    if (_input == Vector3.zero) return;

    //    var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    //}

    private void Move()
    {
        var vector3 = Vector3.Lerp(transform.position, target.position, _speed * Time.deltaTime).normalized;
        _rb.transform.position = (new Vector3(vector3.x, 0f, vector3.z));
    }

}
