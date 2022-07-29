using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePlayer : MonoBehaviour
{
    public GameObject AudioManager;
    // Start is called before the first frame update
    void Start(){   

        AudioManager.GetComponent<AudioManager>().Play("Theme");
    }

    private void OnDestroy() {
        AudioManager.GetComponent<AudioManager>().Stop("Theme");
        return;  
    }
}
