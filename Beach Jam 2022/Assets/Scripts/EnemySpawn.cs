using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemy;
    public Vector3 spawnPoint;

    public EnemySpawn(GameObject newEnemy, Vector3 newSpawnPoint)
    {
        enemy = newEnemy;
        spawnPoint = newSpawnPoint;
    }
}