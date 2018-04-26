using UnityEngine;
using System.Collections;

public class GameMgr : ModuleComponent<GameMgr> {

    Camera __camera;
    public Camera _camera
    {
        get{ return __camera; }
        set { __camera = value; }
    }

    public void Load()
    {
        MapMgr.Instance.Load();
        ActorMgr.Instance.Load();
        SkillMgr.Instance.Load();
        DropOutMgr.Instance.Load();
        TimerMgr.Instance.Load();
    }
    public void Begin()
    {
        MapMgr.Instance.Begin();
        ActorMgr.Instance.Begin();
        SkillMgr.Instance.Begin();
        DropOutMgr.Instance.Begin();
        TimerMgr.Instance.Begin();
    }
    //Temp
    void Update()
    {
        UpdateLogic(Time.deltaTime);
    }
    public void UpdateLogic(float dt)
    {
        ActorMgr.Instance.UpdateLogic(dt);
        SkillMgr.Instance.UpdateLogic(dt);
        TimerMgr.Instance.UpdateLogic(dt);
    }
    void OnGUI()
    {
        if (GUILayout.Button("退出", GUILayout.Width(150), GUILayout.Height(150)))
            Application.Quit();
    }
}
