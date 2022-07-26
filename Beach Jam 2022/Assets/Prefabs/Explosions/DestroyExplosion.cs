using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
        StartCoroutine(DeathTimer());
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitUntil(() => particleSystem.isPlaying == false);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
