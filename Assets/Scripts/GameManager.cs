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

    public GridLayout gridLayout;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = playerManager.GetPlayerPosition();

        Vector2 movementKeyInput = inputManager.CollectMovementKeyInput();
        Vector2 mousePos = inputManager.CollectMousePos();

        if(inputManager.isMouse1Down()){
            Vector2 mousePosToWorld = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int mousePosToGrid = gridLayout.WorldToCell(mousePosToWorld);
            tilemapManager.BreakTile(mousePosToGrid);
        }

        playerManager.MovePlayer(movementKeyInput);
        cameraController.SetPostion(playerPos);

        if(chunkManager){
            HashSet<Chunk> chunkSet = chunkManager.GetChunksInRadius(playerPos);
            chunkManager.ChangeActiveChunks(chunkSet);
            chunkManager.LoadAndDeloadChunks();
        }
        
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireSphere((Vector3)playerManager.GetPlayerPosition(), chunkManager.chunkCheckRadius);
    }
}
