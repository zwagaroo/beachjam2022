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
            Vector3 mousepos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            var newball = Instantiate(ballPrefab, new Vector3(mousepos.x, mousepos.y, mousepos.z), Quaternion.Euler(30,45,0));
            newball.transform.parent = projectileCanvas.transform;

        }
    }
}
