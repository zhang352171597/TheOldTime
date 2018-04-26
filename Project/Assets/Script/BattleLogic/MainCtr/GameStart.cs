using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    void Start()
    {
        DataCenter.Instance.Load();
        GameMgr.Instance.Load();
        UIMgr.Instance.GetUI<BackpackMainUI>().Load();
        var a = new LoadingControl();
        a.Add(GamePath.MapPrefabs + "Map15001");
        a.Begin(_OnFinish, 1);
    }
    void _OnFinish()
    {
        GameMgr.Instance.Begin();
        GameObject.Destroy(gameObject);
    }
}
