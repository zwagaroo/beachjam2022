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
    public Animator tentacleAnim;
    public AnimationClip throwAnimation;
    private bool canThrow = true;
    private bool doneThrowing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            canThrow = false;
            StartCoroutine(ThrowInk());
        }
    }

    public void FinishThrowing()
    {
        doneThrowing = true;
    }

    IEnumerator ThrowInk()
    {
        Vector3 heading = Input.mousePosition - mCamera.WorldToScreenPoint(player.position);
        heading.Normalize();
        tentacleAnim.SetTrigger("isThrowing");
        doneThrowing = false;
        yield return new WaitUntil(() => doneThrowing);
        doneThrowing = false;
        //Vector3 heading = mCamera.ScreenToWorldPoint(Input.mousePosition) - mCamera.ScreenToWorldPoint(player.position);
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
        float angle = Mathf.Rad2Deg * (Mathf.Acos(heading.x / heading.magnitude));
        if (heading.y < 0)
        {
            angle = -angle;
        }
        newball.transform.Rotate(0, 0, angle - 73.158f);
        canThrow = true;
    }
}
