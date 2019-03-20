using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Tab
{
	public string Name;
	public Button SelectButton;
	public GameObject ContentPanel;
}

public class BuildMenu : MonoBehaviour
{
	[SerializeField] private Tab[] _tabs;

	void Start()
	{
//		if (_tabs.Length > 0)
//		{
//			for (int i = 0; i < _tabs.Length; i++)
//			{
//				// Set all tabs inactive by default
//				_tabs[i].ContentPanel.SetActive(false);
//			}
//
//			// Set first tab as active
//			_tabs[0].ContentPanel.SetActive(true);
//		}
//
//		Debug.Log("All UI buttons have been initialized.");
	}
}
