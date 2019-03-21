using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Something
{
    public float abc = 1.23f;
    public bool yes = false;
}

public class BuildablesDataEditor : EditorWindow
{
    private Object jsonObject;

    private List<Mapping> _mappings;
    private List<Something> _somethings = new List<Something>
    {
        new Something(),
        new Something(),
        new Something()
    };

    private int _int = 3;

    [MenuItem("Poly World/Buildables Data Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BuildablesDataEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Custom Buildables JSON Generator", EditorStyles.boldLabel);

        GUILayout.Space(20);

        // Input for saving and loading the file
        jsonObject = EditorGUILayout.ObjectField(jsonObject, typeof(Object), false);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load!"))
        {
            LoadData();
        }
        if (GUILayout.Button("Save!"))
        {
            // TODO: do stuff
        }
        EditorGUILayout.EndHorizontal();

        // Show each item
//        if (_mappings != null)
//        {
//            SerializedObject serializedObject = new SerializedObject(this);
//            SerializedProperty serializedProperty = serializedObject.FindProperty("_mappings");
//            foreach (var VARIABLE in COLLECTION)
//            {
//                
//            }
//        }

//        if (_somethings != null)
//        {
//            SerializedObject obj = new SerializedObject(this);
//            SerializedProperty property = obj.FindProperty("_somethings");
//            EditorGUILayout.PropertyField(property);
//        }
    }

    void LoadData()
    {
        _mappings = JsonUtility.FromJson<MappingLister>(((TextAsset)jsonObject).text).items;

        // loop through each variation and load their prefab from the AssetDatabase
        for (int i = 0; i < _mappings.Count; i++)
        {
            for (int j = 0; j < _mappings[i].variations.Count; j++)
            {
                SubMapping mapping = _mappings[i].variations[j];

//                Debug.Log("Loading prefab at \"" + "Assets/Resources/" + mapping.prefab + "\"");
                mapping.prefabGO = Resources.Load<GameObject>(mapping.prefab);
            }
        }
    }
}
