using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string sceneToLoad;
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
        //This is super hacky but we don't have time. This is so that when player returns to the main screen from pause menu it closes pause menu
        if(this.GetComponentInParent<PauseGame>()){
            this.GetComponentInParent<PauseGame>().StopPausing();
        }
    }

    
}
