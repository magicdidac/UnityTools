using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathGeneratorEditor : EditorWindow
{

    private GameObject pathPref;
    private GameObject pointPathPref;
    private float offset = .5f;
    private List<GameObject> points = new List<GameObject>();

    [MenuItem("Tools/Path Generator Window")]
    public static void ShowWindow()
    {
        GetWindow<PathGeneratorEditor>("Path Generator");
    }

    private void OnGUI()
    {

        if (!pathPref)
            pathPref = Resources.Load<GameObject>("_PathGenerator/Path");

        if(!pointPathPref)
            pointPathPref = Resources.Load<GameObject>("_PathGenerator/PathPoint");

        GUILayout.Label("Select a path to generate");

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create path prefab"))
        {
            GameObject pathPoint = Instantiate(pathPref, Vector3.zero, Quaternion.identity);

            int numOfPaths = GameObject.FindGameObjectsWithTag("Path").Length;

            pathPoint.name = "Path (" + numOfPaths + ")";

        }

        if(GUILayout.Button("Generate path"))
        {
            GeneratePath();
        }

        GUILayout.EndHorizontal();

    }


    private void GeneratePath()
    {

        GameObject path = null;
        try
        {
            path = Selection.gameObjects[0];
        }
        catch { }

        if (!path || path.tag != "Path")
        {
            Debug.LogWarning("Cannot create path.");
            return;
        }

        Transform storage = path.transform.GetChild(0);
        Transform initialPoint = path.transform.GetChild(1);
        Transform finalPoint = path.transform.GetChild(2);

        DeleteOldPath();

        Vector3 line = finalPoint.position - initialPoint.position;
        float distance = Vector3.Distance(finalPoint.position, initialPoint.position);

        for(int i = 0; i < ((int) distance/offset) + 1; i++)
        {
            //Debug.Log("PENE");
            Vector3 position = initialPoint.position + (line.normalized * offset * i);

            GameObject newPoint = Instantiate(pointPathPref, position, Quaternion.identity);
            newPoint.transform.parent = storage;
            points.Add(newPoint);
        }

    }

    private void DeleteOldPath()
    {
        try
        {
            foreach (GameObject g in points)
            {
                if (g.name != "Storage")
                    DestroyImmediate(g);
            }
        }
        catch { }

        points.Clear();
    }


}
