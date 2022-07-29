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
    public GameObject healthBarPrefab; //replace this later with HUD
    public GameObject nextLevelArrowPrefab;
    private GameObject[] nextLevelArrows;

    //Ocean
    public GameObject ocean;
    public bool createNewOceanAtStart = true;
    public float oceanSize = 10f;
    private float oceanHeight = 2f;
    public GameObject oceanPrefab;

    public enum WorldBoundary {UPPER_RIGHT, UPPER_LEFT, LOWER_RIGHT, LOWER_LEFT};
    Dictionary<WorldBoundary, BoxCollider> worldBoundaryColliders = new Dictionary<WorldBoundary, BoxCollider>();
    //public BoxCollider[] worldBoundaryColliders = new BoxCollider[4]; //If createOcean = true, will generate these automatically
    private float worldBoundaryColliderWidth = 1f;
    private float worldBoundaryColliderOffset = 0f;


    //Environmental Prefabs
    public GameObject[] islandPrefabs;
    public GameObject[] rockPrefabs;
    public GameObject[] treePrefabs;

    //Environmental Lighting Assets
    private enum WeatherState{
        CLEAR,
        STORM
    }
    public Material daytimeSkybox;
    public Material spookySkybox;

    //Enemy Spawning
    public GameObject[] enemyTypePrefabs;
    public GameObject navalMinePrefab;
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    public Vector2 spawnRangeMax;
    public Vector2 spawnRangeMin;
    public List<Vector2> spPoints = new List<Vector2>();

    //Implementation with ocean scene
    public WaterVolumeHelper waterVolumeHelper;

    // Start is called before the first frame update
    void Start()
    {
        //Set the LevelManager Singleton Instance
        Instance = this;

        
        //Setup weather
        SetWeather(WeatherState.CLEAR);

        //Create ocean and world colliders
        if(createNewOceanAtStart){
            ocean = Instantiate(oceanPrefab, new Vector3(0.5f, 0, 0.5f), Quaternion.identity);
        }
        SetupOcean();
        waterVolumeHelper = ocean.GetComponent<WaterVolumeHelper>();

        //Setup player
        SetupPlayer();
        GenerateLevel();
        FinishLevel();

    }

    void SetupPlayer(){
        player = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        followCamera.target = player; 
    }

    void SetupOcean(){
        ocean.transform.GetChild(0).localScale = new Vector3(oceanSize, oceanHeight, oceanSize);
        Debug.Log("OCEAN GENERATED");

        float colliderY = 2f;

        //Upper Right
        BoxCollider upper_right = gameObject.AddComponent<BoxCollider>();
        upper_right.center = new Vector3(oceanSize,colliderY,oceanSize/2f);
        upper_right.size = new Vector3(worldBoundaryColliderWidth, 10f, oceanSize);
        worldBoundaryColliders.Add(WorldBoundary.UPPER_RIGHT, upper_right);

        //Upper Left
        BoxCollider upper_left = gameObject.AddComponent<BoxCollider>();
        upper_left.center = new Vector3(oceanSize/2f,colliderY,oceanSize);
        upper_left.size = new Vector3(oceanSize, 10f, worldBoundaryColliderWidth);
        worldBoundaryColliders.Add(WorldBoundary.UPPER_LEFT, upper_left);

        //Lower Right
        BoxCollider lower_right = gameObject.AddComponent<BoxCollider>();
        lower_right.center = new Vector3(oceanSize/2f,colliderY,0f);
        lower_right.size = new Vector3(oceanSize, 10f, worldBoundaryColliderWidth);
        worldBoundaryColliders.Add(WorldBoundary.LOWER_RIGHT, lower_right);

        //Lower Left
        BoxCollider lower_left = gameObject.AddComponent<BoxCollider>();
        lower_left.center = new Vector3(0f,colliderY,oceanSize/2f);
        lower_left.size = new Vector3(worldBoundaryColliderWidth, 10f, oceanSize);
        worldBoundaryColliders.Add(WorldBoundary.LOWER_LEFT, lower_left);




    }

    void SetWeather(WeatherState state){
        if(state == WeatherState.CLEAR){
            RenderSettings.skybox = daytimeSkybox;
        }else if (state == WeatherState.STORM){
            RenderSettings.skybox = spookySkybox;
        }
        DynamicGI.UpdateEnvironment();
    }

    void GenerateLevel(){
        Debug.Log("GENERATING LEVEL");
        SetWorldBoundariesActive(true);
        //SpawnEnemies();
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
        foreach(Collider collider in worldBoundaryColliders.Values){
            collider.isTrigger = !isActive;
        }
    }

    public void FinishLevel()
    {
        Debug.Log("Level complete!");
        SetWorldBoundariesActive(false);
        //TODO: Instantiate and populate nextLevelArrows here
    }

    void OnTriggerEnter(Collider other){ //When player leaves map, create new level
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("PLAYER MOVED TO NEW LEVEL");
            player.transform.position = playerSpawn.position;
            //endArrow.SetActive(false);
            enemies.Clear(); //DO WE NEED THIS?
            GenerateLevel();

        }
    }

    Vector3 getSpawnPosOppositeWorldBoundary(float mapSize, WorldBoundary exitWorldBoundary){
        float offsetFromWorldEdge = 2f;

        switch(exitWorldBoundary){ //Based on the direction the player just exited the map
            case WorldBoundary.UPPER_RIGHT: //Spawn player in lower left
                break;
            case WorldBoundary.UPPER_LEFT: //Spawn player in lower right
            
                break; 
            case WorldBoundary.LOWER_RIGHT: //Spawn player in upper left
            
                break;
            case WorldBoundary.LOWER_LEFT: //Spawn player in upper right
            
                break;
        }
        return Vector3.zero;
 /*        //Lower Left
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
        worldBoundaryColliders[3].size = new Vector3(worldBoundaryColliderWidth, 10f, oceanSize); */
    }
}
