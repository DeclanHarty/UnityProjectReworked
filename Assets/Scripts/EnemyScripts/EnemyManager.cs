using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> activeEnemies;
    
    public TilemapManager tilemapManager;
    public GameManager gameManager;

    public GameObject enemyPrefab;

    public int minSpawnDistance;
    public int maxSpawnDistance;
    public float despawnDistance;

    public int maxSpawnedEnemies;

    private float timeSinceLastSpawn;
    public float timeBetweenSpawns;

    public void UpdateManager(Vector2 playerPosition){
        foreach(Enemy enemy in activeEnemies){
            enemy.StateUpdate(playerPosition);
        }
        

        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > timeBetweenSpawns){
            DespawnCheck(playerPosition);
            SpawnCheck(playerPosition);
            timeSinceLastSpawn = 0;
        }
    }

    public void FixedUpdateManager(Vector2 playerPosition){
        foreach(Enemy enemy in activeEnemies){
            enemy.StateFixedUpdate(playerPosition);
        }
    }

    public void InjectTilemapManager(TilemapManager tilemapManager){
        this.tilemapManager = tilemapManager;
    }

    public void InjectGameManager(GameManager gameManager){
        this.gameManager = gameManager;
    }

    private void SpawnCheck(Vector2 playerPosition){
        if(activeEnemies.Count < maxSpawnedEnemies){
            List<Vector2Int> availableTiles = GetAvailableSpawnPoints(playerPosition);
            int chosenTileIndex = Random.Range(0, availableTiles.Count);
            Vector2 spawnPosition = tilemapManager.CellToWorldPosition(availableTiles[chosenTileIndex]);
            SpawnEnemy(spawnPosition);
        }
    }

    private void DespawnCheck(Vector2 playerPosition){
        Vector2Int playerTilePos = tilemapManager.WorldToCellPosition(playerPosition);
        List<Enemy> enemiesForRemoval = new List<Enemy>();
        for(int i = 0; i < activeEnemies.Count; i++){
            Enemy enemy = activeEnemies[i];
            Vector2Int enemyTilePos = tilemapManager.WorldToCellPosition(enemy.GetPosition());
            if(Vector2Int.Distance(playerTilePos, enemyTilePos) > despawnDistance){
                activeEnemies.Remove(enemy);
                enemy.DeleteEnemy();
            } 
        }

        foreach(Enemy enemy in enemiesForRemoval){
            activeEnemies.Remove(enemy);
        }
    }

    private void SpawnEnemy(Vector2 spawnPosition){
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.InjectEnemyManager(this);
        activeEnemies.Add(enemyScript); 
    }

    private List<Vector2Int> GetAvailableSpawnPoints(Vector2 playerPosition){
        List<Vector2Int> availableSpawnPoints = new List<Vector2Int>();
        Vector2Int playerPositionInTiles = tilemapManager.WorldToCellPosition(playerPosition);
        int spawnRadius = Random.Range(minSpawnDistance, maxSpawnDistance + 1);
        List<Vector2Int> perimeter = MidpointCircle.FindPerimeter(playerPositionInTiles, spawnRadius);
        foreach(Vector2Int pos in perimeter){
            if(tilemapManager.GetTileValue(pos) == -1){
                availableSpawnPoints.Add(pos);
            }
        }

        return availableSpawnPoints;  
    }

    public void NotifyEnemyDeath(Enemy enemy){
        activeEnemies.Remove(enemy);
    }

    public void EnemyDamagedPlayer(int damage){
        gameManager.PlayerTookDamage(damage);
    }

    public void PauseEnemies(){
        foreach(Enemy enemy in activeEnemies){
            enemy.StopRBVelocity();
        }
    }
}
