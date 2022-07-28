using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavalMine : MonoBehaviour
{
	public GameObject explosionPrefab;
	
    IEnumerator Start()
    {
        //Testing Code, comment out later
        yield return new WaitForSeconds(3f);
        Explode();
    }

    void Update()
    {
        
    }
	
	void Explode()
	{
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
