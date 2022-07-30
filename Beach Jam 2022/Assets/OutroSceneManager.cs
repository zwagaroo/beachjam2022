using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroSceneManager : MonoBehaviour
{
    public DialogueManager dialogueManager; //DialogueManager prefab should be a child of this object
    public ExplosionDeath prisoncell;
    public GameObject endImage;

    IEnumerator Start() //OUTRO SCENE SEQUENCE
    {
        yield return new WaitForSeconds(1f);
        prisoncell.Death();
        yield return new WaitForSeconds(1f);
        var player = Object.FindObjectOfType<PlayerController>();
        player.GetComponent<PlayerController>().inputDisabled = true;
        dialogueManager.StartDialogue(DialogueManager.DialogueType.ENDING, gameObject);
        yield return new WaitUntil(() => !dialogueManager.dialogueActive);
        endImage.SetActive(true);

    }


    //Once intro dialogue is over, start actual game scene.

}