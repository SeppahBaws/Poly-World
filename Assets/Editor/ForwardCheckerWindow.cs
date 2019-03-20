using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ForwardCheckerWindow : EditorWindow
{
    private Transform p1;
    private Transform p2;

    private string result = "Hello World!";

    [MenuItem("Poly World/Testings/Forward Checker")]
    public static void ShowWindow()
    {
        GetWindow<ForwardCheckerWindow>("Forward checker!");
    }

    void OnGUI()
    {
        p1 = EditorGUILayout.ObjectField("Point 1", p1, typeof(Transform), true) as Transform;
        p2 = EditorGUILayout.ObjectField("Point 2", p2, typeof(Transform), true) as Transform;

        if (GUILayout.Button("Calculate!"))
            CalculateDirection();

        GUILayout.Label("Result: " + result);
    }

    void CalculateDirection()
    {
        Vector3 vec = p2.position - p1.position;
        vec.Normalize();

        float val = Vector3.Dot(p1.right, vec);
        Debug.Log("Dotproduct: " + val);

        if (val >= -0.2f && val <= 0.2f)
        {
            result = "Front";
            Debug.Log("Object is in front!");
        }
        else if (val > 0.2f)
        {
            result = "Right!";
            Debug.Log("Object is to the right!");
        }
        else if (val < -0.2f)
        {
            result = "Left!";
            Debug.Log("Object is to the left!");
        }
    }
}
