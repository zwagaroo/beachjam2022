using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDeath : MonoBehaviour
{
    public GameObject explosionPrefab;
    //public float timeToDie = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(DeathTimer(timeToDie));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeathTimer(float time)
    {
        yield return new WaitForSeconds(time);
        var explosionObject = Instantiate(explosionPrefab, transform.position, transform.rotation);
        var explosion = explosionObject.GetComponent<ParticleSystem>();
        Destroy(this.gameObject);
        //yield return new WaitUntil(() => explosion.isPlaying == false);
        Debug.Log("explosion done");
        
    }

    public void Death()
    {
        //yield return new WaitForSeconds(time);
        var explosionObject = Instantiate(explosionPrefab, transform.position, this.transform.rotation);
        //var explosion = explosionObject.GetComponent<ParticleSystem>();
        Destroy(this.gameObject);
        //yield return new WaitUntil(() => explosion.isPlaying == false);
        Debug.Log("explosion done");
    }
}
