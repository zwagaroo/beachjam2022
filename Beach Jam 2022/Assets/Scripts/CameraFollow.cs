using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
 
    // Update is called once per frame
    void Update()
    {
        if(target){
            float height = transform.position.y;
            Vector3 targetPos = target.transform.position;
            transform.position = new Vector3(targetPos.x, height, targetPos.z);
        }
    }
}
