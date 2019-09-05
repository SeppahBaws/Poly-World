using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public World(int width, int height)
    {
        WorldWidth = width;
        WorldHeight = height;

        InitializeTiles();
    }

    public int WorldWidth { get; private set; }
    public int WorldHeight { get; private set; }
    public Tile[,] WorldData { get; private set; }
    public Building[] Buildings { get; private set; }
	// public BuildableObject[] Objects { get; private set; }

    void InitializeTiles()
    {
        WorldData = new Tile[WorldWidth, WorldHeight];

        for (int x = 0; x < WorldWidth; x++)
        {
            for (int y = 0; y < WorldHeight; y++)
            {
                WorldData[x, y] = new Tile(x, y);
            }
        }
    }

    public void SetTileAt(Vector2 position, Tile.TileType type)
    {
        WorldData[(int)position.x, (int)position.y].SetType(type);
    }

    public Tile GetTileAt(Vector2 position)
    {
        return WorldData[(int)position.x, (int)position.y];
	    // return null;
    }

    public List<Tile> GetTilesFrom(Vector2 position, int width, int height)
    {
        List<Tile> tiles = new List<Tile>();

        if (width == 1 && height == 1)
        {
            tiles.Add(GetTileAt(position));
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int newX = (int) position.x + x;
                    int newY = (int) position.y + y;
                    tiles.Add(GetTileAt(new Vector2(newX, newY)));
                }
            }
        }

        return tiles;
    }
    
    public List<Tile> GetNeighbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();

        // North
        if (tile.Y < WorldHeight - 1)
            neighbors.Add(GetTileAt(new Vector2(tile.X, tile.Y + 1)));

        // East
        if (tile.X < WorldWidth - 1)
            neighbors.Add(GetTileAt(new Vector2(tile.X + 1, tile.Y)));

        // South
        if (tile.Y > 0)
            neighbors.Add(GetTileAt(new Vector2(tile.X, tile.Y - 1)));

        // West
        if (tile.X > 0)
            neighbors.Add(GetTileAt(new Vector2(tile.X - 1, tile.Y)));

        return neighbors;
    }
}
