using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildController
{
    public static void Build(TileMapping mapping, Tile selected, World world)
    {
        if (mapping.type == Tile.TileType.Empty)
            return;
        
        Debug.Log("Mapping Size: (" + mapping.width + ", " + mapping.height + ")");

        for (int x = 0; x < mapping.width; x++)
        {
            for (int y = 0; y < mapping.height; y++)
            {
                Tile tile = world.GetTileAt(new Vector2(selected.X, selected.Y));
                if (tile.Type != Tile.TileType.Empty)
                    return;
            }
        }
        
        for (int x = 0; x < mapping.width; x++)
        {
            for (int y = 0; y < mapping.height; y++)
            {
                Tile tile = world.GetTileAt(new Vector2(selected.X + x, selected.Y + y));
                tile.SetType(mapping.type);
                tile.SetParentTile(selected);
            }
        }

        WorldController.Instance.SpawnInstance(new Vector3(selected.X, 0, selected.Y), mapping.prefab.transform);
    }

    public static void Demolish(Tile tile, World world)
    {
        if (tile.ParentTile != null)
        {
            Tile parentTile = tile.ParentTile;

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
            tile.SetType(Tile.TileType.Empty);
            
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

}


































