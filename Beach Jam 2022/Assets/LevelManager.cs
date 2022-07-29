using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bitgem.VFX.StylisedWater;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    //Required setup
    public CameraFollow followCamera;

    //Player Character
    public GameObject playerPrefab;
    private GameObject player;
    public Transform playerSpawn;

    //UI
    public GameObject healthBarPrefab;

    //Ocean
    public GameObject ocean;
    public bool createNewOceanAtStart = true;
    public float oceanSize = 10f;
    private float oceanHeight = 2f;
    public GameObject oceanPrefab;

    public BoxCollider[] worldBoundaryColliders = new BoxCollider[4]; //If createOcean = true, will generate these automatically
    private float worldBoundaryColliderWidth = 1f;
    private float worldBoundaryColliderOffset = 0f;

    //Environmental Prefabs
    public GameObject[] islandPrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] treePrefabs;

    //Environmental Lighting Assets
    public Material daytimeSkybox;
    public Material spookySkybox;

    //Enemy Spawning
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
        //Set the LevelManager Singleton Instance
        Instance = this;

        //Create ocean and world colliders
        if(createNewOceanAtStart){
            ocean = Instantiate(oceanPrefab, new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
        }
        SetupOcean();
        waterVolumeHelper = ocean.GetComponent<WaterVolumeHelper>();

        //Setup player
        SetupPlayer();
        GenerateLevel();
    }

    void SetupPlayer(){
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        //followCamera.target = player; //TODO: FIX LATER
    }

    void SetupOcean(){
        ocean.transform.GetChild(0).localScale = new Vector3(oceanSize, oceanHeight, oceanSize);
        Debug.Log("OCEAN GENERATED");

        float colliderY = 2f;
        worldBoundaryColliders = new BoxCollider[4];

        //Lower Left
        worldBoundaryColliders[0] = gameObject.AddComponent<BoxCollider>();
        worldBoundaryColliders[0].center = new Vector3(0f,colliderY,oceanSize/2f);
        worldBoundaryColliders[0].size = new Vector3(worldBoundaryColliderWidth, 10f, oceanSize);

        //Lower Right
        worldBoundaryColliders[1] = gameObject.AddComponent<BoxCollider>();
        worldBoundaryColliders[1].center = new Vector3(oceanSize/2f,colliderY,0f);
        worldBoundaryColliders[1].size = new Vector3(oceanSize, 10f, worldBoundaryColliderWidth);

        //Upper Left
        worldBoundaryColliders[2] = gameObject.AddComponent<BoxCollider>();
        worldBoundaryColliders[2].center = new Vector3(oceanSize/2f,colliderY,oceanSize);
        worldBoundaryColliders[2].size = new Vector3(oceanSize, 10f, worldBoundaryColliderWidth);

        //Upper Right
        worldBoundaryColliders[3] = gameObject.AddComponent<BoxCollider>();
        worldBoundaryColliders[3].center = new Vector3(oceanSize,colliderY,oceanSize/2f);
        worldBoundaryColliders[3].size = new Vector3(worldBoundaryColliderWidth, 10f, oceanSize);


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
        SetWorldBoundariesActive(false);
        endArrow.SetActive(true);
    }

    void OnTriggerEnter(Collider other){ //When player leaves map, create new level
        if(other.gameObject.tag == "Player")
        {
            player.transform.position = playerSpawn.position;
            endArrow.SetActive(false);
            enemies.Clear(); //DO WE NEED THIS?
            GenerateLevel();
        }
    }
}
