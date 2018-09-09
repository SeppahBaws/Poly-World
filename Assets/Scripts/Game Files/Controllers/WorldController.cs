using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldController : MonoBehaviour
{
    public World world;

    public static WorldController Instance { get; private set; }

    public static BuildController BuildController { get; private set; }

    public Tile.TileType SelectedTileType;

    public List<TileMapping> TileMappings;
    public List<Mapping> Mappings;
    public TextAsset tileMappingsSource;
    public GameObject worldParent;
    public List<Transform> worldObjects;

    private void Start()
    {
        LoadMappings();
        Instance = this;

        gameObject.AddComponent<BuildController>();
        BuildController = GetComponent<BuildController>();
    }

    // Load the mappings from the JSON file
    private void LoadMappings()
    {
        Mappings = JsonUtility.FromJson<MappingLister>(tileMappingsSource.text).items;

        // loop through each variation and load their prefab from the AssetDatabase
        for (int i = 0; i < Mappings.Count; i++)
        {
            for (int j = 0; j < Mappings[i].variations.Count; j++)
            {
                SubMapping mapping = Mappings[i].variations[j];

                mapping.prefabGO = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + mapping.prefab + ".prefab", typeof(GameObject));
            }
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

        if (Input.GetMouseButton(0))
            Build();

        if (Input.GetMouseButton(1))
            Demolish();
    }

    // Temporary code to select what to build
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

        //Debug.Log(mapping.ToString());

        BuildController.Build(mapping, clickedTile, world);
    }

    private void Demolish()
    {
        Vector3 clickPoint = CastRay();
        Tile clickedTile = world.GetTileAt(V3ToWorldSpace(clickPoint));

        BuildController.Demolish(clickedTile, world);
    }

    public void SpawnInstance(Vector3 pos, Transform obj, Tile tile)
    {
        Transform spawnedObj = Instantiate(obj, pos, Quaternion.identity, worldParent.transform);
        worldObjects.Add(spawnedObj);
        tile.ObjectInScene = spawnedObj.gameObject;
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
        return new Vector2(v.x, v.z);
    }


    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying)
        {
            foreach (Tile tile in world.WorldData)
            {
                switch (tile.Type)
                {
                    case Tile.TileType.Empty:
                        Gizmos.color = Color.white;
                        break;
                    case Tile.TileType.Road:
                        Gizmos.color = Color.grey;
                        break;
                    case Tile.TileType.House:
                    case Tile.TileType.Apartment:
                        Gizmos.color = Color.red;
                        break;
                    case Tile.TileType.Factory:
                        Gizmos.color = Color.yellow;
                        break;
                }

                Gizmos.DrawCube(new Vector3(tile.X + 0.5f, 0, tile.Y + 0.5f), Vector3.one / 2);
            }
        }
    }
}