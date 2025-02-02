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
    public CaveGenerator caveGenerator;
    public CameraController cameraController;
    public EnemyManager enemyManager;

    public GridLayout gridLayout;

    // Start is called before the first frame update
    void Awake()
    {
        MapInfo mapInfo = caveGenerator.SetMap();
        playerManager.InjectInputManager(inputManager);
        playerManager.SwitchState(new FreeMovement());
        Vector2 playerWorldPosition = tilemapManager.CellToWorldPosition(mapInfo.spawnPosInTilemap);
        playerManager.SetPlayerPosition(playerWorldPosition);
        cameraController.SetPostion(playerWorldPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = playerManager.GetPlayerPosition();
        // if(inputManager.isMouse1Down()){
        //     Vector2 mousePosToWorld = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
        //     Vector3Int mousePosToGrid = gridLayout.WorldToCell(mousePosToWorld);
        //     tilemapManager?.BreakTile(mousePosToGrid);
        //
        playerManager.UpdatePlayer();
        cameraController?.MoveCamera(playerPos);

        if(chunkManager){
            HashSet<Chunk> chunkSet = chunkManager.GetChunksInRadius(playerPos);
            chunkManager.ChangeActiveChunks(chunkSet);
            chunkManager.LoadAndDeloadChunks();
        }

        enemyManager.UpdateManager(playerManager.GetPlayerPosition());
    }

    void FixedUpdate(){
        playerManager.FixedUpdatePlayer();
        enemyManager.FixedUpdateManager(playerManager.GetPlayerPosition());
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireSphere((Vector3)playerManager.GetPlayerPosition(), chunkManager.chunkCheckRadius);
    }
}
