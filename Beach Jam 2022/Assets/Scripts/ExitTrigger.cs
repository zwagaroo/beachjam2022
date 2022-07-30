using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitTrigger : MonoBehaviour
{
    public string sceneToLoad;

    void OnTriggerEnter(Collider other){ //When player leaves map, create new level
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER MOVING TO NEW LEVEL");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
