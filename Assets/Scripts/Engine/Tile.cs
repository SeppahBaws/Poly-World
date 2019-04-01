using System.ComponentModel.Design;
using UnityEngine;

public class Tile
{
	public Tile(int x, int y, TileType type = TileType.Empty, Tile parentTile = null, GameObject objectInScene = null)
	{
        X = x;
        Y = y;
        Type = type;
        ParentTile = parentTile;
		ObjectInScene = objectInScene;
	}

	public enum TileType
	{
		Empty = 0,
		Road = 1,
		House = 2,
		Apartment = 3,
		Factory = 4,
		CityHall = 5,
		PoliceStation = 6,
		Store = 7,
	}

	public Tile pathParent;
	public int f, g, h;
	public int X { get; private set; }
    public int Y { get; private set; }
    public TileType Type { get; private set; }
    public Tile ParentTile { get; private set; }
	public GameObject ObjectInScene { get; set; }

    public void SetType(TileType type)
    {
        Type = type;
    }

    public void SetParentTile(Tile parent)
    {
        if (parent != null
            && X == parent.X && Y == parent.Y)
            return;
        ParentTile = parent;
    }

    public Vector2 Position()
    {
        return new Vector2(X, Y);
    }

    public Vector3 WorldPosition()
    {
        return new Vector3(X, 0, Y);
    }

    public Vector3 AdjustedWorldPosition()
    {
        return new Vector3(X + 0.5f, 0, Y + 0.5f);
    }


    // =============
    //   Overrides
    // =============
    public override string ToString()
    {
        return "position: (" + X + "," + Y + "), type: " + Type;
    }
}
