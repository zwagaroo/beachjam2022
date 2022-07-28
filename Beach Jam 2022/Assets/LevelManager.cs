using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public GameObject player;
    public Camera mCamera;
    public Collider exit;
    public GameObject[] enemyTypes;
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    public Transform playerSpawn;
    public Vector2 spawnRangeMax;
    public Vector2 spawnRangeMin;
    public List<Vector2> spPoints = new List<Vector2>();
    public GameObject endArrow;
    // Start is called before the first frame update
    void Start()
    {
        endArrow.SetActive(false);
        levelManager = this;
        player.transform.position = playerSpawn.position;
        exit.isTrigger = false;
        enemies.Clear();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        int numberOfEnemies = Random.Range(2, 7);
        Debug.Log(numberOfEnemies + " enemies");
        spPoints = PoissonDiscSampling.GeneratePoints(5f, spawnRangeMax - spawnRangeMin, 30);

        for (int i=0; i < numberOfEnemies; i+= 1)
        {
            int enemyIndex = Random.Range(0, enemyTypes.Length);
            Debug.Log(enemyIndex);
            GameObject enemy = enemyTypes[enemyIndex];

            int spIndex = Random.Range(0, spPoints.Count);
            Vector2 spawn = spPoints[spIndex];
            Vector3 spPt = new Vector3(spawn.x - ((spawnRangeMax.x-spawnRangeMin.x)/2), 1, spawn.y - ((spawnRangeMax.y - spawnRangeMin.y) / 2));
            EnemySpawn enemySp = new EnemySpawn(enemy, spPt);
            enemies.Add(enemySp);
        }
        foreach (EnemySpawn enemySp in enemies)
        {
            GameObject temp = Instantiate(enemySp.enemy, enemySp.spawnPoint, Quaternion.Euler(0,0,0));
            if(temp.GetComponent<MinionController>() != null)
            {
                temp.GetComponent<MinionController>().target = player.transform;
            }

            if(temp.GetComponentInChildren<Turret>() != null)
            {
                temp.GetComponentInChildren<Turret>().target = player.transform;
            }

            if(temp.GetComponentInChildren<HealthbarFollow>() != null)
            {
                temp.GetComponentInChildren<HealthbarFollow>().mCamera = mCamera;
            }

        } 
    }

    public void NextLevel()
    {
        Debug.Log("Level complete!");
        exit.isTrigger = true;
        endArrow.SetActive(true);
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            player.transform.position = playerSpawn.position;
            exit.isTrigger = false;
            endArrow.SetActive(false);
            enemies.Clear();
            SpawnEnemies();
            Debug.Log("Level exited!");
        }
    }
}
