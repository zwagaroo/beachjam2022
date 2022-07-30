using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _o_ : MonoBehaviour
{
    public AudioManager am; 

    // Start is called before the first frame update
    void Start()
    {
        AudioManager am = FindObjectOfType<AudioManager>(); 
        am.Play(":)");
    }

    // Update is called once per frame
}
