using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
    }
}
