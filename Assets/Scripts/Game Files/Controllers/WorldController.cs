using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEditor;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public World world;
    
    public static WorldController Instance { get; protected set; }

    public Tile.TileType SelectedTileType;

    public List<TileMapping> TileMappings;
    public TextAsset tileMappingsSource;
    public GameObject worldParent;
    public List<Transform> worldObjects;

    private void Start()
    {
        //LoadMappings();
        Instance = this;
    }
    
    private void LoadMappings()
    {
        TileMappingLister lister = JsonUtility.FromJson<TileMappingLister>(tileMappingsSource.text);

        foreach (TileMapping mapping in lister.mappings)
        {
            TileMappings.Add(new TileMapping(
                mapping.name,
                mapping.width,
                mapping.height,
                (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + mapping.prefab + ".prefab", typeof(GameObject)),
                (Tile.TileType)mapping.type));
        }
    }

    public void GenerateWorld(int width, int height)
    {
        world = new World(width, height);
        Debug.Log("New world created! Size: (" + width + ", " + height + ")");
    }

    private void Update()
    {
        HandleSelectedTileTypeSwitching();

        if (Input.GetMouseButtonDown(0))
            Build();

        if (Input.GetMouseButtonDown(1))
            Demolish();
    }

    private void HandleSelectedTileTypeSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            SelectedTileType = (Tile.TileType)0;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            SelectedTileType = (Tile.TileType)1;
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            SelectedTileType = (Tile.TileType)2;
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            SelectedTileType = (Tile.TileType)3;
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            SelectedTileType = (Tile.TileType)4;
    }

    private void Build()
    {
        Vector3 clickPoint = CastRay();
        Tile clickedTile = world.GetTileAt(V3ToWorldSpace(clickPoint));
        TileMapping mapping = TileMappings.Find(m => m.type == SelectedTileType);
        
        Debug.Log(mapping.ToString());

        BuildController.Build(mapping, clickedTile, world);
    }

    private void Demolish()
    {
        Vector3 clickPoint = CastRay();
        Tile clickedTile = world.GetTileAt(V3ToWorldSpace(clickPoint));
        
        BuildController.Demolish(clickedTile, world);
    }

    public void SpawnInstance(Vector3 pos, Transform obj)
    {
        Transform spawnedObj = Instantiate(obj, pos, Quaternion.identity, worldParent.transform);
        worldObjects.Add(spawnedObj);
    }
    
    
    // ==================
    //   Util functions
    // ==================
    Vector3 CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        
        return Vector3.zero;
    }

    Vector2 V3ToWorldSpace(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
}