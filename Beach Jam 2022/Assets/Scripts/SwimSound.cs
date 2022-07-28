using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimSound : MonoBehaviour
{
    public AudioSource aSource;
    public AudioClip[] swimSounds;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!aSource.isPlaying && player.isMoving)
        {
            int rand = Random.Range(0, swimSounds.Length);
            aSource.PlayOneShot(swimSounds[rand]);
        }
    }
}
