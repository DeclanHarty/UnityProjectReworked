using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InputManager inputManager;
    public PlayerManager playerManager;
    public ChunkManager chunkManager;
    public TilemapManager tilemapManager;
    public CameraController cameraController;
    public EnemyManager enemyManager;
    public UIManager uIManager;

    private GameState gameState;

    // Start is called before the first frame update
    void Awake()
    {
        // Create and Set Map
        MapInfo mapInfo = CaveGenerator.CreateMap(new Vector2Int(75,75), new Vector2Int(5,3));
        tilemapManager.Set2DMap(mapInfo.GetMap());
        tilemapManager.SetNavGraph(mapInfo.GetNavGraph());

        // Get Player Spawn and Move player and camera 
        playerManager.InjectInputManager(inputManager);
        playerManager.InjectUIManager(uIManager);
        
        playerManager.SwitchState(new FreeMovement());
        Vector2 playerWorldPosition = tilemapManager.CellToWorldPosition(mapInfo.spawnPosInTilemap);
        playerManager.SetPlayerPosition(playerWorldPosition);
        cameraController.SetPostion(playerWorldPosition);

        SwitchState(new Playing());
    }

    // Update is called once per frame
    void Update()
    {
        gameState.StateUpdate();
    }

    void FixedUpdate(){
        gameState.StateFixedUpdate();
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireSphere((Vector3)playerManager.GetPlayerPosition(), chunkManager.chunkCheckRadius);
    }

    public void PlayerTookDamage(int damage){
        if(playerManager.TakeDamage(damage)){
            SwitchState(new GameOver());
        }
    }

    public void SwitchState(GameState gameState){
        this.gameState = gameState;
        gameState.InjectGameManager(this);
        gameState.OnStateChange();
    }
}
