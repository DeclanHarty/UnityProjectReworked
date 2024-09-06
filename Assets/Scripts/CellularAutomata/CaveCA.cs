using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaveCA
{
    // This is a Cellular Automata Algorithm for trying to generate cave structures.

    // It currently uses this algorithm
    // 4-8 / 4, 6 / 2 / M (Number of 'Live' neighbors for a 'live' cell to survive, Number of 'Live' neighbors for a 'dead' cell to become alive, Number of States [Dead and Alive], Neighbor Procedure [Moore's : All neighbors horizontal, vertical, and diagonal])
    // 
    public static bool[] RunTurn(bool[] currentState, int width){
        bool[] newState = new bool[currentState.Length];

        for(int y = 0; y < currentState.Length / width; y++){
            for(int x = 0; x < width; x++){
                int numOfLiveNeighbors = 0;
                bool currentCellIsAlive = false;


                for(int neighborY = y - 1; neighborY <= y + 1; neighborY++){
                    for(int neighborX = x - 1; neighborX <= x + 1; neighborX++){
                        if(neighborX == x && neighborY == y){
                            currentCellIsAlive = currentState[y * width + x];
                        }

                        if(neighborY * width + neighborX < 0 || neighborY * width + neighborX >= currentState.Length){
                            numOfLiveNeighbors++;
                            continue;
                        }

                        if(currentState[neighborY * width + neighborX]){
                            numOfLiveNeighbors++;
                        }
                    }
                }

                if(currentCellIsAlive){
                    if(numOfLiveNeighbors >= 4){
                        newState[y * width + x] = true;
                    }else{
                        newState[y * width + x] = false;
                    }
                }else{
                    if(numOfLiveNeighbors >= 4 ){
                        newState[y * width + x] = true;
                    }else{
                        newState[y * width + x] = false;
                    }
                }
            }
        }

        return newState;
    }
}
