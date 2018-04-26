using UnityEngine;
using System.Collections;

public static class EditorDebug{

	public static void Log(string str)
    {
        Debug.Log(str);
    }
    public static void LogError(string str)
    {
        Debug.LogError("-------------------Error---------------" + str);
    }
    public static void LogWarning(string str)
    {
        Debug.LogWarning("-------------------Warning---------------" + str);
    }
}
