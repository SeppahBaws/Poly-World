using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    public GameObject ObjectInScene { get; private set; }
    public Tile.TileType Type { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Building(Tile.TileType type, int width = 1, int height = 1)
    {
        Type = type;
        Width = width;
        Height = height;
    }
}