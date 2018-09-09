using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MappingLister
{
    public List<Mapping> items;

    public MappingLister(List<Mapping> items)
    {
        this.items = items;
    }
}

[System.Serializable]
public class Mapping
{
    public string name;
    public int width;
    public int height;
    public List<SubMapping> variations;

    public Mapping(string name, int width, int height, List<SubMapping> variations)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.variations = variations;
    }
}

[System.Serializable]
public class SubMapping
{
    public string name;
    public string prefab;
    public GameObject prefabGO;

    public SubMapping(string name, string prefab)
    {
        this.name = name;
        this.prefab = prefab;
    }
}


[System.Serializable]
public class TileMapping
{
    public string name;
    public int width;
    public int height;
    public GameObject prefab;
    public Tile.TileType type;

    public TileMapping()
    {
        name = "";
        width = 0;
        height = 0;
        type = Tile.TileType.Empty;
    }

    public TileMapping(string name, int width, int height, GameObject prefab, Tile.TileType type)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.prefab = prefab;
        this.type = type;
    }

    public override string ToString()
    {
        return "name: '" + name + "', width: " + width + ", height: " + height + ", prefab name: '" + prefab.name +
               "', type: '" + type + "'";
    }
}

// Old system
[System.Serializable]
public class TileMappingLister
{
    public List<TileMapping> mappings;

    public TileMappingLister(List<TileMapping> mappings)
    {
        this.mappings = mappings;
    }
}

[System.Serializable]
public class TempMapping
{
    public string name;
    public int width;
    public int height;
    public GameObject prefab;
    public Tile.TileType type;

    public TempMapping()
    {
        name = "";
        width = 0;
        height = 0;
        type = Tile.TileType.Empty;
    }

    public TempMapping(string name, int width, int height, GameObject prefab, Tile.TileType type)
    {
        this.name = name;
        this.width = width;
        this.height = height;
        this.prefab = prefab;
        this.type = type;
    }
}

[System.Serializable]
public class TempMappingLister
{
    public List<TempMapping> mappings;

    public TempMappingLister(List<TempMapping> mappings)
    {
        this.mappings = mappings;
    }
}