using UnityEngine;
using System.Collections;

public class TestInstance : ModuleComponent<TestInstance> {

    public GameObject testObj;
    public void Create()
    {
        var obj = Resources.Load(GamePath.ActorPrefabs + "defaultModel") as GameObject;
        testObj = GameObject.Instantiate(obj);
    }

    public void Log()
    {
        Debug.Log("testObj:   " + testObj == null);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.LoadLevel("test2");
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            Log();
        }
    }
}
