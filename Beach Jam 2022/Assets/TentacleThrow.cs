using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TentacleThrow : MonoBehaviour
{
    public GameObject ballPrefab;
    public Camera mCamera;
    public Transform player;
    public Canvas projectileCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        { 
            //Vector3 heading = mCamera.ScreenToWorldPoint(Input.mousePosition) - mCamera.ScreenToWorldPoint(player.position);
            Vector3 heading = Input.mousePosition - mCamera.WorldToScreenPoint(player.position);
            heading.Normalize();
            //RaycastHit hit;
            //Ray ray = mCamera.ScreenPointToRay(Input.mousePosition);
            //if(Physics.Raycast(ray, out hit))
            //{
            //    Vector3 DirectionVector = hit.point - player.position;
            //}
           //Vector3 mousepos = mCamera.ScreenToWorldPoint(Input.mousePosition);
           var newball = Instantiate(ballPrefab, player.position, player.rotation);
            //newball.GetComponent<InkProjectile>().target = new Vector3(heading.y*Mathf.Cos(30)*Mathf.Sin(45) - heading.x*Mathf.Sin(45), 2f, heading.y * Mathf.Cos(30) * Mathf.Sin(45) + heading.x * Mathf.Sin(45));
            newball.GetComponent<InkProjectile>().target = new Vector3(heading.y * Mathf.Sin(45) + heading.x * Mathf.Sin(45), 0, heading.y * Mathf.Sin(45) - heading.x * Mathf.Sin(45));
        }
    }
}
