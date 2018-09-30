using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficController : MonoBehaviour
{
    public GameObject carPrefab;

    private Tile startTile;
    private Tile endTile;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 hitpos = WorldController.Instance.CastRay();
            startTile = WorldController.Instance.world.GetTileAt(new Vector2(hitpos.x, hitpos.z));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Vector3 hitpos = WorldController.Instance.CastRay();
            endTile = WorldController.Instance.world.GetTileAt(new Vector2(hitpos.x, hitpos.z));
            RequestRoute(startTile, endTile);
        }
    }

    // Spawns a car and makes it drive from its start to its goal.
    // TODO: more functionality later. e.g. specifying what type etc
    public void RequestRoute(Tile start, Tile goal)
    {
        GameObject car = Instantiate(carPrefab);
        car.GetComponent<Vehicle>().path = AStar.FindPath(start, goal);
        car.transform.position = new Vector3(start.Position().x + 0.5f, 0, start.Position().y + 0.5f);
    }
}