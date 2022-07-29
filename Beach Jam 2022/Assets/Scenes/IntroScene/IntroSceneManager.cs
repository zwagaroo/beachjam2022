using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager; //DialogueManager prefab should be a child of this object

    IEnumerator Start() //INTRO SCENE SEQUENCE
    {
        yield return new WaitForSeconds(1f);
        dialogueManager.StartDialogue(DialogueManager.DialogueType.INTRO, gameObject);
    }

    //Once intro dialogue is over, start actual game scene.
    public void HandleEndDialogue(){
        SceneManager.LoadScene("OceanScene");
    }

}
