using UnityEngine;

public class Tile
{
	public Tile(int x, int y, TileType type = TileType.Empty, Tile parentTile = null)
	{
        X = x;
        Y = y;
        Type = type;
        ParentTile = parentTile;
	}

	public enum TileType
	{
		Empty = 0,
		Road = 1,
		House = 2,
		Factory = 3,
		Test = 4
	}

	public int X { get; private set; }
    public int Y { get; private set; }
    public TileType Type { get; private set; }
    public Tile ParentTile { get; private set; }

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


    // ============
    //   Override
    // ============
    public override string ToString()
    {
        return "position: (" + X + "," + Y + "), type: " + Type;
    }
}
