using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemy;
    public Transform spawnPoint;
}

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public Camera mCamera;
    public Collider exit;
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    // Start is called before the first frame update
    void Start()
    {
        exit.isTrigger = false;
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        foreach (EnemySpawn enemySp in enemies)
        {
            GameObject temp = Instantiate(enemySp.enemy, enemySp.spawnPoint.position, enemySp.spawnPoint.rotation);
            temp.GetComponent<MinionController>().target = player.transform;
            temp.GetComponentInChildren<HealthbarFollow>().mCamera = mCamera;
        } 
    }

    void NextLevel()
    {
        exit.isTrigger = true;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Level exited!");
        }
    }
}
