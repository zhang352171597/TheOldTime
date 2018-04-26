using UnityEngine;
using System.Collections;

public class ModuleComponent<Type> : MonoBehaviour where Type : MonoBehaviour
{
    private static Type _instance;
    public static Type Instance
    {
        get
        {
            _instance = FindObjectOfType<Type>();
            if(_instance == null)
            {
                _instance = new GameObject(typeof(Type).Name).AddComponent<Type>();
                GameObject.DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }
}