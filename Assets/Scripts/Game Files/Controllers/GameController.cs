using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int worldWidth;
    public int worldHeight;

    public WorldController worldController;
    
    void Start()
    {
        worldController.GenerateWorld(worldWidth, worldHeight);
    }
}