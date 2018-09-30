using System;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    North,
    East,
    South,
    West
}

struct Neighbor
{
    public Tile tile;
    public Direction direction;

    public Neighbor(Tile tile, Direction direction)
    {
        this.tile = tile;
        this.direction = direction;
    }
}

public class BuildController : MonoBehaviour
{
    public void Build(TileMapping mapping, Tile selected, World world)
    {
        if (mapping.type == Tile.TileType.Empty)
            return;

        // TODO: can we optimize this?
        // Do a check
        for (int x = 0; x < mapping.width; x++)
        {
            for (int y = 0; y < mapping.height; y++)
            {
                Tile tile = world.GetTileAt(new Vector2(selected.X + x, selected.Y + y));
                if (tile.Type != Tile.TileType.Empty)
                    return;
            }
        }

        // Actually do stuff
        for (int x = 0; x < mapping.width; x++)
        {
            for (int y = 0; y < mapping.height; y++)
            {
                Tile tile = world.GetTileAt(new Vector2(selected.X + x, selected.Y + y));
                tile.SetType(mapping.type);
                tile.SetParentTile(selected);
            }
        }

        if (selected.Type != Tile.TileType.Road)
        {
            Debug.Log(mapping.prefab);
            WorldController.Instance.SpawnInstance(new Vector3(selected.X, 0, selected.Y), mapping.prefab.transform, selected);
        }
        else
        {
            UpdateRoad(selected, world);
        }


        if (mapping.type == Tile.TileType.Road)
        {
            List<Neighbor> neighbors = GetNeighborRoads(selected, world);
            List<int> neighborPos = GetNeighborRoadsInts(selected, world);

            for (int i = 0; i < neighbors.Count; i++)
            {
                UpdateRoad(neighbors[i].tile, world);
            }
        }
    }

    public void Demolish(Tile tile, World world)
    {
        if (tile.ParentTile != null)
        {
            Tile parentTile = tile.ParentTile;

            Destroy(parentTile.ObjectInScene);

            for (int x = 0; x < world.WorldWidth; x++)
            {
                for (int y = 0; y < world.WorldHeight; y++)
                {
                    Tile tempTile = world.WorldData[x, y];
                    if (tempTile.ParentTile != null
                        && tempTile.ParentTile.Position() == parentTile.Position())
                    {
                        tempTile.SetParentTile(null);
                        tempTile.SetType(Tile.TileType.Empty);
                    }
                }
            }

            parentTile.SetType(Tile.TileType.Empty);
        }
        else
        {
            Tile.TileType prevType = tile.Type;

            tile.SetType(Tile.TileType.Empty);

            if (prevType == Tile.TileType.Road)
            {
                List<Neighbor> neighbors = GetNeighborRoads(tile, world);
                List<int> neighborPos = GetNeighborRoadsInts(tile, world);

                for (int i = 0; i < neighbors.Count; i++)
                {
                    UpdateRoad(neighbors[i].tile, world);
                }
            }

            Destroy(tile.ObjectInScene);

            for (int x = 0; x < world.WorldWidth; x++)
            {
                for (int y = 0; y < world.WorldHeight; y++)
                {
                    Tile tempTile = world.WorldData[x, y];

                    if (tempTile.ParentTile != null
                        && tempTile.ParentTile.X == tile.X && tempTile.ParentTile.Y == tile.Y)
                    {
                        tempTile.SetParentTile(null);
                        tempTile.SetType(Tile.TileType.Empty);
                    }
                }
            }
        }
    }

    private void UpdateRoad(Tile tile, World world)
    {
        List<int> neighbors = GetNeighborRoadsInts(tile, world);
        Debug.Log("Number of neighbors: " + neighbors.Count);

        Destroy(tile.ObjectInScene);

        GameObject newRoad = null;

        switch (neighbors.Count)
        {
            case 0:
                newRoad = Instantiate(WorldController.Instance.Mappings[0].variations[0].prefabGO);
                newRoad.transform.position = new Vector3(tile.Position().x, 0, tile.Position().y);
                break;
            case 1:
                // default is horizontal road, doesn't need a rotation
                newRoad = Instantiate(WorldController.Instance.Mappings[0].variations[0].prefabGO);
                newRoad.transform.position = new Vector3(tile.Position().x, 0, tile.Position().y);

                // if the neighbor has an odd number, the connection has to be vertical so it gets a 90 degree rotation
                if (neighbors[0] % 2 != 0)
                {
                    newRoad.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    newRoad.transform.position += new Vector3(0, 0, 1);
                }
                break;
            case 2:
                if (neighbors[0] == 1 && neighbors[1] == 3 || neighbors[0] == 2 && neighbors[1] == 4)
                {
                    // The two neighbors are in a line, use straight road
                    newRoad = Instantiate(WorldController.Instance.Mappings[0].variations[0].prefabGO);
                    newRoad.transform.position = new Vector3(tile.Position().x, 0, tile.Position().y);

                    // Rotate as needed
                    if (neighbors[0] == 1)
                    {
                        newRoad.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        newRoad.transform.position += new Vector3(0, 0, 1);
                    }
                }
                else
                {
                    // The two neighbors are not in a straight line, use a corner road and rotate as needed
                    newRoad = Instantiate(WorldController.Instance.Mappings[0].variations[1].prefabGO);
                    newRoad.transform.position = new Vector3(tile.Position().x, 0, tile.Position().y);

                    // Standard rotation:
                    // --+
                    //   |

                    // Rotate
                    if (neighbors[0] == 1 && neighbors[1] == 2)
                    {
                        // |
                        // +--

                        newRoad.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                        newRoad.transform.position += new Vector3(1, 0, 1);
                    }
                    else if (neighbors[0] == 2 && neighbors[1] == 3)
                    {
                        // +--
                        // |

                        newRoad.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
                        newRoad.transform.position += new Vector3(1, 0, 0);
                    }
                    //else if (neighbors[0] == 3 && neighbors[1] == 4)
                    //{
                        // --+
                        //   |

                        // no rotation required
                    //}
                    else if (neighbors[0] == 1 && neighbors[1] == 4)
                    {
                        //   |
                        // --+

                        newRoad.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        newRoad.transform.position += new Vector3(0, 0, 1);
                    }
                }
                break;
            case 3:
                // Use 3-way crossing road, rotate as needed
                // rotations shown in diagram

                newRoad = Instantiate(WorldController.Instance.Mappings[0].variations[2].prefabGO);
                newRoad.transform.position = new Vector3(tile.Position().x, 0, tile.Position().y);

                // Standard rotation:
                // --+--
                //   |

                // Rotate
                if (neighbors[0] == 1 && neighbors[1] == 2 && neighbors[2] == 4)
                {
                    //   |
                    // --+--

                    newRoad.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                    newRoad.transform.position += new Vector3(1, 0, 1);
                }
                else if (neighbors[0] == 1 && neighbors[1] == 2 && neighbors[2] == 3)
                {
                    // |
                    // +---
                    // |

                    newRoad.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
                    newRoad.transform.position += new Vector3(1, 0, 0);
                }
                //else if (neighbors[0] == 2 && neighbors[1] == 3 && neighbors[2] == 4)
                //{
                    // --+--
                    //   | 
                    
                    // no rotation required
                //}
                else if (neighbors[0] == 1 && neighbors[1] == 3 && neighbors[2] == 4)
                {
                    //   |
                    // --+
                    //   |

                    newRoad.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    newRoad.transform.position += new Vector3(0, 0, 1);
                }
                break;
            case 4:
                // Use 4-way crossing road
                newRoad = Instantiate(WorldController.Instance.Mappings[0].variations[3].prefabGO);
                newRoad.transform.position = new Vector3(tile.Position().x, 0, tile.Position().y);
                break;
        }
        tile.ObjectInScene = newRoad;
        newRoad.transform.parent = WorldController.Instance.worldParent.transform;
    }

    private List<Neighbor> GetNeighborRoads(Tile tile, World world)
    {
        List<Neighbor> neighbors = new List<Neighbor>();

        // North
        if (tile.Y < world.WorldHeight - 1)
        {
            Tile neighbor = world.GetTileAt(new Vector2(tile.X, tile.Y + 1));
            if (neighbor.Type == Tile.TileType.Road)
                neighbors.Add(new Neighbor(neighbor, Direction.North));
        }

        // East
        if (tile.X < world.WorldWidth - 1)
        {
            Tile neighbor = world.GetTileAt(new Vector2(tile.X + 1, tile.Y));
            if (neighbor.Type == Tile.TileType.Road)
                neighbors.Add(new Neighbor(neighbor, Direction.East));
        }

        // South
        if (tile.Y > 0)
        {
            Tile neighbor = world.GetTileAt(new Vector2(tile.X, tile.Y - 1));
            if (neighbor.Type == Tile.TileType.Road)
                neighbors.Add(new Neighbor(neighbor, Direction.South));
        }

        // West
        if (tile.X > 0)
        {
            Tile neighbor = world.GetTileAt(new Vector2(tile.X - 1, tile.Y));
            if (neighbor.Type == Tile.TileType.Road)
                neighbors.Add(new Neighbor(neighbor, Direction.West));
        }

        return neighbors;
    }

    /**
     * This function returns a List of integers. It works as follows:
     * Each direction is given a number:
     * - North: 1
     * - East:  2
     * - South: 3
     * - West:  4
     * This allows for easy checking in which direction a road is, by just simple even/odd checking or something similar.
     *
     * The list is also always sorted.
     *
     * It searches for neighboring roads in each direction.
     * If it finds one, it puts the number of the direction it found the road at in a list that it then returns in the end.
     */
    private List<int> GetNeighborRoadsInts(Tile tile, World world)
    {
        List<int> neighborPos = new List<int>();

        if (tile.Y < world.WorldHeight - 1)
        {
            if (world.GetTileAt(new Vector2(tile.X, tile.Y + 1)).Type == Tile.TileType.Road)
                neighborPos.Add(1);
        }

        // East
        if (tile.X < world.WorldWidth - 1)
        {
            if (world.GetTileAt(new Vector2(tile.X + 1, tile.Y)).Type == Tile.TileType.Road)
                neighborPos.Add(2);
        }

        // South
        if (tile.Y > 0)
        {
            if (world.GetTileAt(new Vector2(tile.X, tile.Y - 1)).Type == Tile.TileType.Road)
                neighborPos.Add(3);
        }

        // West
        if (tile.X > 0)
        {
            if (world.GetTileAt(new Vector2(tile.X - 1, tile.Y)).Type == Tile.TileType.Road)
                neighborPos.Add(4);
        }

        return neighborPos;
    }
}
