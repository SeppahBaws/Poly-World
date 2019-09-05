using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Object", menuName = "Poly World/Buildable Object", order = 1)]
public class BuildableObject : ScriptableObject
{
	public enum ItemCategory
	{
		None,
		Infrastructure,
		Residential,
		Commercial,
		Industrial,
		Service
	}

	public ItemCategory Category = ItemCategory.None;
	public string Name = "";
	public int Cost = 0;
	public int MaxBuildTimes = -1; // -1 indicates infinite amount of times
	public List<GameObject> Prefabs;
}
