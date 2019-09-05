using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WorldController : MonoBehaviour
{
	public TextAsset testAsset;

    public bool ShowDebugGrid;

    public World world;

    public static WorldController Instance { get; private set; }

    [SerializeField] private BuildController _buildController;

    public Tile.TileType SelectedTileType;

    public bool loadMappingsOnGameLoad;
    public TextAsset tileMappingsSource;
    public List<Mapping> Mappings;
    public GameObject worldParent;
    public List<Transform> worldObjects;

    public static Tile GetClickedTile()
    {
        Vector3 clickPos = Instance.CastRay();
        return Instance.world.GetTileAt(Instance.V3ToWorldSpace(clickPos));
    }

    public static Mapping GetSelectedMapping()
    {
        return Instance.Mappings.Find(m => m.assignedType == Instance.SelectedTileType);
    }

    private void Start()
    {
        if (loadMappingsOnGameLoad)
            LoadMappings();
        Instance = this;
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

				Debug.Log("Loading prefab at \"" + "Assets/Resources/" + mapping.prefab + "\"");
                mapping.prefabGO = Resources.Load<GameObject>(mapping.prefab);
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
        // if (Input.GetMouseButton(0))
        // {
		// 	// Ignore UI clicks
		// 	if (EventSystem.current.IsPointerOverGameObject())
		// 		return;

		// 	Build();
        // }
        // else if (Input.GetMouseButton(1))
        // {
	    //     Demolish();
        // }
    }

	// Used by build menu UI
    public void SwitchBuildType(int type)
    {
        _buildController.SetMapping(Mappings[type - 1]);
	    SelectedTileType = (Tile.TileType)type;
    }

    // private void Build()
    // {
    //     Vector3 clickPoint = CastRay();
    //     Tile clickedTile = world.GetTileAt(V3ToWorldSpace(clickPoint));
    //     Mapping mapping = Mappings.Find(m => m.assignedType == SelectedTileType);

    //     // TODO: Make a clear distinction between which tool does what.
    //     // The build controller for instance should handle mouse inputs
    //     // and then decide what it wants to do with that
    //     BuildController.Build(mapping, clickedTile, world);
    // }

    // private void Demolish()
    // {
    //     Vector3 clickPoint = CastRay();
    //     Tile clickedTile = world.GetTileAt(V3ToWorldSpace(clickPoint));

    //     BuildController.Demolish(clickedTile, world);
    // }

    public void SpawnInstance(Vector3 pos, Transform obj, Tile tile)
    {
        Transform spawnedObj = Instantiate(obj, pos, Quaternion.identity, worldParent.transform);
        worldObjects.Add(spawnedObj);
        tile.ObjectInScene = spawnedObj.gameObject;
    }


    // ==================
    //   Util functions
    // ==================
    public Vector3 CastRay()
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


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		if (EditorApplication.isPlaying && ShowDebugGrid)
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
#endif
}