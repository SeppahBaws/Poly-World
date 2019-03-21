using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public int worldWidth;
    public int worldHeight;

//    public GameMode Mode
//    {
//        get { return Mode; }
//        set
//        {
//            Mode = value; HandleModeChange(Mode);
//        }
//    }

    public WorldController worldController;

    public enum GameMode
    {
        Inspect,
        Build,
        Camera
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        worldController.GenerateWorld(worldWidth, worldHeight);
    }

    private void HandleModeChange(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Inspect:
                UIController.Instance.SetUIModeInspect();
                break;
            case GameMode.Build:
                UIController.Instance.SetUIModeBuild();
                break;
            case GameMode.Camera:
                UIController.Instance.SetUIModeCamera();
                break;
            default:
                return;
        }
    }
}