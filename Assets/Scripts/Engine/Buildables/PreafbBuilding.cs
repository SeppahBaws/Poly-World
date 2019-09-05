using UnityEngine;

// PrefabBuilding
//  -> main building
//  -> attached prefabs -> script manages when they should be rendered (LOD system, but on the CPU side -> Can be super heavy?)
//  -> category

// ResidentialHouse : PrefabBuilding
//  -> main building = mesh
//  -> attached prefabs = 

[System.Serializable]
public class PreafbBuilding
{
    public GameObject buildingPrefab;
    public GameObject[] detailPrefabs;
    public BuildingCategory category;
}
