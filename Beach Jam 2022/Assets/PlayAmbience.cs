using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbience : MonoBehaviour
{
    public AudioManager am;
    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        am.Play("Ambience");
    }
}
