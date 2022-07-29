using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{

    public GameObject AudioManager;
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.GetComponent<AudioManager>().Play("Onwards");
    }

    void Update(){
        if(Input.GetKeyDown("space")){
            AudioManager.GetComponent<AudioManager>().Play("CannonShot");
        }
    }

}
