using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    public static List<Tile> FindPath(Tile start, Tile goal)
    {
        List<Tile> openSet = new List<Tile>();
        List<Tile> closedSet = new List<Tile>();
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            Tile q = openSet.OrderBy(t => t.f).First();
            openSet.Remove(q);
            closedSet.Add(q);

            if (q == goal)
            {
                return RetracePath(start, goal);
            }

            foreach (Tile neighbor in WorldController.Instance.world.GetNeighbors(q))
            {
                if (neighbor.Type != Tile.TileType.Road || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = q.g + GetDistance(q, neighbor);
                if (newCostToNeighbor < neighbor.g || !openSet.Contains(neighbor))
                {
                    neighbor.g = newCostToNeighbor;
                    neighbor.h = GetDistance(neighbor, goal);
                    neighbor.pathParent = q;
                    
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        
        return new List<Tile>();
    }

    private static List<Tile> RetracePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.pathParent;
        }
        
        path.Reverse();
        return path;
    }

    private static int GetDistance(Tile tileA, Tile tileB)
    {
        int dstX = Mathf.Abs(tileA.X - tileB.X);
        int dstY = Mathf.Abs(tileA.Y - tileB.Y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}