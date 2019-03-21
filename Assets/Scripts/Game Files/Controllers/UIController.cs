using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] private GameObject _buildMenu;
    [SerializeField] private GameObject _buildMenuButton;

    void Awake()
    {
        Instance = this;
    }

    public void SetUIModeInspect()
    {
        throw new System.NotImplementedException();
    }

    public void SetUIModeBuild()
    {
        throw new System.NotImplementedException();
    }

    public void SetUIModeCamera()
    {
        throw new System.NotImplementedException();
    }
}
