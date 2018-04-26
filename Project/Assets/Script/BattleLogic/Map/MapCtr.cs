using UnityEngine;
using System.Collections;


public class MapCtr : MonoBehaviour 
{
    string _mapID;
    public string mapID
    { get { return _mapID; } }
    TransferCtr[] _transfers;
    string _mapName;
    public string mapName
    { get { return _mapName;} }
    enDropData _dropData;
    public enDropData dropData
    { get { return _dropData; } }

    MapActorSetting[] _actos;
	public void Begin()
    {
        _transfers = GetComponentsInChildren<TransferCtr>();
        for (int i = 0; i < _transfers.Length; ++i )
        {
            _transfers[i].Begin();
        }
        _mapID = gameObject.name.Substring(3);
        _mapName = JSONMap.getInstance().GetName(mapID);
        var dropStr =  JSONMap.getInstance().GetDrop(mapID);
        _dropData = new enDropData(dropStr);
        UIMgr.Instance.GetUI<BattleMainUI>().mapModule.ResetData(this);
        _actos = GetComponentsInChildren<MapActorSetting>();
        for (int i = 0; i < _actos.Length; ++i)
        {
            _actos[i].Create();
        }
        EditorDebug.Log("Begin A Map of ID: " + _mapID);
    }
    public TransferCtr GetTransferByID(string id)
    {
        for(int i = 0 ; i < _transfers.Length; ++i)
        {
            if (_transfers[i].mapID == id)
                return _transfers[i];
        }
        EditorDebug.LogWarning("在该地图下没找到ID为" + id + "的传送门");
        return null;
    }
    public void Despawn()
    {
        for (int i = 0; i < _actos.Length; ++i)
        {
            _actos[i].Despawn();
        }
        ObjManager.Instance.Despawn(gameObject);
    }
}
