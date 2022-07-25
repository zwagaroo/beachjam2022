using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarFollow : MonoBehaviour
{
    //How high above the character the slider spawns
    public float height;

    //Adjust the slider if it is offcenter
    public float xalignment;
    
    public Camera mCamera;

    //Target character
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = mCamera.WorldToScreenPoint(target.position);
        this.transform.position = mCamera.ScreenToWorldPoint(new Vector3(pos.x + xalignment, pos.y + height, pos.z));
    }
}
