using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //Holds the lines currently used
    private Line[] lines;
    
    public GameObject dialogueBox;
    public Text dialogueText;

    //Array of chars for text
    public char[] sentenceArray;

    public GameObject nameBox;
    public Text nameText;

    public Image portrait;
    public Sprite takoyakiPortrait;
    public Sprite surferPortrait;

    public enum DialogueType {INTRO, ENDING};

    //Holds lines for intro and ending
    public Line[] intro;
    public Line[] ending;

    //Check if the dialogue manager is active
    public bool dialogueActive;

    //Record which line you are on
    public int lineCounter;

    //detect when you are on the last line so you can end dialogue
    public bool lastLine;

    //force player to wait until the character is finished talking
    public bool canContinue = true;

    private GameObject callbackObject; //Object for DialogueManager to notify via BroadcastMessage that dialogue is over

    void Start(){
        //StartDialogue(DialogueType.INTRO); //DEBUG only
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canContinue)
        {
            if (lastLine)
            {
                EndDialogue();
            }
            else
            {
                StartCoroutine(NextSentence());
                Debug.Log("Continue Dialogue");
            }
        }
    }

    public void StartDialogue(DialogueType dialogueType, GameObject myCallbackObject=null)
    {
        if(dialogueType == DialogueType.INTRO){
            lines = intro;
        }else if(dialogueType == DialogueType.ENDING){
            lines = ending;
        }
        canContinue = true;

        callbackObject = myCallbackObject;

        dialogueActive = true;
        dialogueBox.SetActive(true);
        nameBox.SetActive(true);
        portrait.gameObject.SetActive(true);
        StartCoroutine(NextSentence());
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        nameBox.SetActive(false);
        portrait.gameObject.SetActive(false);
        dialogueActive = false;
        lineCounter = 0;
        lastLine = false;
        if(callbackObject){
            callbackObject.BroadcastMessage("HandleEndDialogue");
        }
    }

    IEnumerator NextSentence()
    {
        canContinue = false;
        //dialogueText.text = lines[lineCounter].text;
        dialogueText.text = "";
        nameText.text = lines[lineCounter].name;
        portrait.sprite = lines[lineCounter].portrait;

        sentenceArray = lines[lineCounter].text.ToCharArray();

        for (int i = 0; i < lines[lineCounter].text.Length; i++)
        {
            dialogueText.text += sentenceArray[i];
            yield return new WaitForSeconds(.02f);
        }

        
        
        lineCounter += 1;
        if(lineCounter == lines.Length)
        {
            lastLine = true;
        }
        canContinue = true;

    }
}
