using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem particleSystem;

    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(explosionSound, 1.5f);
        particleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(DeathTimer());
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitUntil(() => particleSystem.isPlaying == false);
        
        Destroy(this.gameObject);
    }
}
