using UnityEditor;
using UnityEngine;

public class MissingScriptFinder
{
    [MenuItem("Tools/Find Missing Scripts In Scene")]
    static void FindMissingScripts()
    {
        GameObject[] gos = Object.FindObjectsByType<GameObject>(
            FindObjectsSortMode.None);

        foreach (GameObject go in gos)
        {
            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.Log(
                        $"Missing Script ¹ß°ß: {GetFullPath(go)}",
                        go);
                }
            }
        }
    }

    static string GetFullPath(GameObject go)
    {
        string path = go.name;
        Transform current = go.transform.parent;

        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }

        return path;
    }
}