using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public enum GameMode
	{
		Inspect = 0,
		Build = 1,
		Destroy = 2,
		Camera = 3
	}

	public static GameController Instance { get; private set; }

    public int WorldWidth;
    public int WorldHeight;

	[SerializeField] private GameMode _gameMode;
    public GameMode Mode
    {
        get { return _gameMode; }
        set
        {
            _gameMode = value;
            HandleModeChange(_gameMode);
        }
    }

    public WorldController WorldController;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        WorldController.GenerateWorld(WorldWidth, WorldHeight);
    }

    public void UI_SetMode(int modeId)
    {
        Mode = (GameMode)modeId;
    }

    public void UI_ToggleBuild()
    {
        bool wasBuild = Mode == GameMode.Build;
        Mode = wasBuild ? GameMode.Inspect : GameMode.Build;
        UIController.Instance.GetBuildMenuGO().SetActive(!wasBuild);
    }

    private void HandleModeChange(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Inspect:
//                UIController.Instance.SetUIModeInspect();
                GetComponent<BuildController>().Disable();
                break;
            case GameMode.Build:
				// TODO: not ideal, find a better architecture in which this is much easier to handle.
	            GetComponent<BuildController>().StartBuild();
	            break;
			case GameMode.Destroy:
//                UIController.Instance.SetUIModeBuild();
                GetComponent<BuildController>().StartDestroy();
                break;
            case GameMode.Camera:
//                UIController.Instance.SetUIModeCamera();
                break;
            default:
                return;
        }
    }
}