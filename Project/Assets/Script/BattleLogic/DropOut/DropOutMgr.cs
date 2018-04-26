using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DropOutMgr : ModuleComponent<DropOutMgr>
{
    /// <summary>
    /// 缓存掉落物品索引
    /// </summary>
    List<DropItemCtr> _dropCache;
    public void Load()
    {
        _dropCache = new List<DropItemCtr>();
    }
    public void Begin()
    {

    }
    public void CreateItem(enDropData.strDropResult dropItem, Vector3 bornPos ,Action gainCallback)
    {
        var obj = ObjManager.Instance.addChild("", GamePrefab.DropItem , null);
        var ctr = obj.GetComponent<DropItemCtr>();
        ctr.Begin(dropItem, bornPos, gainCallback, _RemoveDrop);
        _dropCache.Add(ctr);
    }
    void _RemoveDrop(DropItemCtr drop)
    {
        ObjManager.Instance.Despawn(drop.gameObject);
        if (_dropCache.Contains(drop))
            _dropCache.Remove(drop);
    }
    public void ClearAllDrop()
    {
        for(int i= 0 ; i < _dropCache.Count; ++i)
        {
			_dropCache [i].OnDespawn ();
            ObjManager.Instance.Despawn(_dropCache[i].gameObject);
        }
        _dropCache.Clear();
    }
}
