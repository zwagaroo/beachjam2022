using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bitgem.VFX.StylisedWater;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    
    public GameObject playerPrefab;
    private GameObject player;
    public Transform playerSpawn;

    public GameObject ocean;
    public bool createOcean = true;
    public GameObject oceanPrefab;

    public Collider[] worldBoundaryColliders = Collider[4];
    public GameObject[] enemyTypePrefabs;
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    public Vector2 spawnRangeMax;
    public Vector2 spawnRangeMin;
    public List<Vector2> spPoints = new List<Vector2>();
    public GameObject endArrow;

    //Implementation with ocean scene
    public WaterVolumeHelper waterVolumeHelper;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = this;

        //Create ocean and world colliders
        if(createOcean){
            ocean = Instantiate(oceanPrefab, Vector3.zero, Quaternion.zero);
        }
        waterVolumeHelper = ocean.GetComponent<WaterVolumeHelper>();

        //Setup player
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.zero);
        
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateLevel(){
        Debug.Log("GENERATING LEVEL");
        SetWorldBoundariesActive(true);
        SpawnEnemies();
        endArrow.SetActive(false); //TODO: turn this into multiple end arrows
    }

    void SpawnEnemies()
    {
        int numberOfEnemies = Random.Range(2, 7); //Generate random number of enemies
        Debug.Log(numberOfEnemies + " enemies");
        spPoints = PoissonDiscSampling.GeneratePoints(5f, spawnRangeMax - spawnRangeMin, 30);

        for (int i=0; i < numberOfEnemies; i+= 1)
        {
            int enemyIndex = Random.Range(0, enemyTypePrefabs.Length);
            Debug.Log(enemyIndex);
            GameObject enemy = enemyTypePrefabs[enemyIndex];

            int spIndex = Random.Range(0, spPoints.Count);
            Vector2 spawn = spPoints[spIndex];
            Vector3 spPt = new Vector3(spawn.x - ((spawnRangeMax.x-spawnRangeMin.x)/2), 1, spawn.y - ((spawnRangeMax.y - spawnRangeMin.y) / 2));
            EnemySpawn enemySp = new EnemySpawn(enemy, spPt);
            enemies.Add(enemySp);
        }
        foreach (EnemySpawn enemySp in enemies)
        {
            GameObject temp = Instantiate(enemySp.enemy, enemySp.spawnPoint, Quaternion.Euler(0,0,0));
            if(temp.GetComponent<EnemyController>() != null)
            {
                temp.GetComponent<EnemyController>().target = player.transform;
            }

            if(temp.GetComponentInChildren<Turret>() != null)
            {
                temp.GetComponentInChildren<Turret>().target = player.transform;
            }


        } 
    }

    public void SetWorldBoundariesActive(bool isActive){
        foreach(Collider collider in worldBoundaryColliders){
            collider.isTrigger = !isActive;
        }
    }

    public void FinishLevel()
    {
        Debug.Log("Level complete!");
        exit.isTrigger = true;
        endArrow.SetActive(true);
    }

    void OnTriggerEnter(Collider other) //When player leaves map, create new level
        if(other.gameObject.tag == "Player")
        {
            player.transform.position = playerSpawn.position;
            endArrow.SetActive(false);
            enemies.Clear(); //DO WE NEED THIS?
            GenerateLevel();
        }
    }
}
