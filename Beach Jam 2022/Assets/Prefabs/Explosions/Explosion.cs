using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem particleSystem;

    public AudioClip explosionSound;
    public AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        am.Play("Explosion");
        particleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(DeathTimer());
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitUntil(() => particleSystem.isPlaying == false);
        
        Destroy(this.gameObject);
    }
}
