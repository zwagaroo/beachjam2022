using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavalMine : MonoBehaviour
{
	public GameObject explosionPrefab;
	
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //Testing Code
        yield return new WaitForSeconds(3f);
        Explode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void Explode()
	{
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		// Create Explosion here
		Destroy(gameObject);
	}
}
