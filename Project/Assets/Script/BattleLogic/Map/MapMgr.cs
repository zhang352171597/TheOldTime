using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class MapMgr : ModuleComponent<MapMgr> 
{

    /// <summary>
    /// 目标地图ID
    /// </summary>
    string _targetID = "";
    /// <summary>
    /// 当前所处ID
    /// </summary>
    string _lastID = "";
    /// <summary>
    /// 当前地图ID
    /// </summary>
    MapCtr _currentMapCtr;

    public void Load()
    {
        
    }

    public void Begin()
    {
        _targetID = "15001";
        _CreateWorldCamera();
        _CreateNewMap();
    }
    public void Go(string targetID)
    {
        if (_currentMapCtr != null)
        {
             if(_currentMapCtr.mapID == targetID)
                 return;
             _lastID = _currentMapCtr.mapID;
        }

        _targetID = targetID;
        var a = new LoadingControl();
        a.Add(GamePath.MapPrefabs + "Map" + targetID);
        var monsterPathsNode = JSONNode.Parse(JSONMap.getInstance().GetMonsterInfo(_targetID));
        for (int i = 0; i < monsterPathsNode.Count; ++i)
        {
            a.Add(GamePath.ActorPrefabs + monsterPathsNode[i]);
        }
        a.Begin(_CreateNewMap, 0.5f);
    }
    void _CreateWorldCamera()
    {
        var obj = ObjManager.Instance.addChild("", GamePrefab.WorldCameraModule, transform);
        GameMgr.Instance._camera = obj.GetComponentInChildren<Camera>();
    }
    void _CreateNewMap()
    {
        Debug.Log("进入新的地图  " + _targetID);
        DropOutMgr.Instance.ClearAllDrop();
        if (_currentMapCtr != null)
            _currentMapCtr.Despawn();
        var obj = ObjManager.Instance.addChild(GamePath.MapPrefabs, "Map" + _targetID, transform);
        _currentMapCtr = obj.GetComponent<MapCtr>();
        _currentMapCtr.Begin();
        if(_lastID.Length > 0)
        {
            var transfer = _currentMapCtr.GetTransferByID(_lastID);
            if (transfer != null)
                MessageCenter.Instance.Execute(enGameMessageType.setPlayerPosForce, new System.Object[] { transfer.standPos });
            else
            {
                MessageCenter.Instance.Execute(enGameMessageType.setPlayerPosForce, new System.Object[] { Vector3.zero });
                Debug.LogWarning("目标地图不存在传送回路，主角默认位置归零");
            }
        }
        UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.mapName, _currentMapCtr.mapName, 2);
    }
    
    public List<enDropData.strDropResult> dropResult
    {
        get
        {
            return _currentMapCtr.dropData.dropResult;
        }
    }

}
