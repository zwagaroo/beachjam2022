using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager; //DialogueManager prefab should be a child of this object

    // Start is called before the first frame update
    IEnumerator Start() //INTRO SCENE SEQUENCE
    {
        yield return new WaitForSeconds(1f);
        dialogueManager.StartDialogue(DialogueManager.DialogueType.INTRO);
    }

}
