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

    // O(height * width)
    public static bool[] RunTurn(bool[] currentState, int width){
        bool[] newState = new bool[currentState.Length];

        for(int y = 0; y < currentState.Length / width; y++){ // O(height)
            for(int x = 0; x < width; x++){ // O(width)
                int numOfLiveNeighbors = 0;
                bool currentCellIsAlive = false;


                for(int neighborY = y - 1; neighborY <= y + 1; neighborY++){ // O(8)
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

    public static bool[,] Run2DTurn(bool[,] currentState, int width, int height){
        bool[,] newState = new bool[width, height];

        for(int y = 0; y < height; y++){ // O(height)
            for(int x = 0; x < width; x++){ // O(width)
                int numOfLiveNeighbors = 0;
                bool currentCellIsAlive = false;


                for(int neighborY = y - 1; neighborY <= y + 1; neighborY++){ // O(8)
                    for(int neighborX = x - 1; neighborX <= x + 1; neighborX++){
                        if(neighborX == x && neighborY == y){
                            currentCellIsAlive = currentState[x,y];
                        }

                        if(neighborY < 0 || neighborY >= height || neighborX < 0 || neighborX >= width){
                            numOfLiveNeighbors++;
                            continue;
                        }

                        if(currentState[neighborX,neighborY]){
                            numOfLiveNeighbors++;
                        }
                    }
                }

                if(currentCellIsAlive){
                    if(numOfLiveNeighbors >= 4){
                        newState[x,y] = true;
                    }else{
                        newState[x,y] = false;
                    }
                }else{
                    if(numOfLiveNeighbors >= 4){
                        newState[x,y] = true;
                    }else{
                        newState[x,y] = false;
                    }
                }
            }
        }

        return newState;
    }
}
