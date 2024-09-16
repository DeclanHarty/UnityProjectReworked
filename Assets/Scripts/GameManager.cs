using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InputManager inputManager;
    public PlayerManager playerManager;
    public ChunkManager chunkManager;
    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = playerManager.GetPlayerPosition();
        Vector2 input = inputManager.CollectMovementKeyInput();
        playerManager.MovePlayer(input);
        cameraController.SetPostion(playerPos);

        // HashSet<Chunk> chunkSet = chunkManager.GetChunksInRadius(playerPos);
        // chunkManager.ChangeActiveChunks(chunkSet);
        // chunkManager.LoadAndDeloadChunks();
    }

    void OnDrawGizmos(){
        //Gizmos.DrawWireSphere((Vector3)playerManager.GetPlayerPosition(), chunkManager.chunkCheckRadius);
    }
}
