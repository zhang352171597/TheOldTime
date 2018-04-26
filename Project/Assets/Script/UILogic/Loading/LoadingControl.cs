using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
// var a = new LoadingControl();
// a.Add(GamePath.SkillPrefabs + "skill_16001-A");
// a.Begin();
/// </summary>
public class LoadingControl
{
    /// <summary>
    /// 预加载数据
    /// </summary>
    private class enPreLoadData
    {
        public string prefabPath;
        public float progress;
        public bool isUI;

        public void Load(ref int currentCount)
        {
            var obj = ObjManager.Instance.addChild("" , prefabPath , null);
            ObjManager.Instance.Despawn(obj);
            currentCount++;
        }
    }

    LoadingUI _loadingUI;
    Dictionary<string , enPreLoadData> _preLoadDatas;
    Action _onFinish;

    bool _isComplete;
    int _currentCount;
    /// <summary>
    /// 加载结束缓冲等待计数器
    /// </summary>
    float _finishReadyMax;

    public LoadingControl()
    {
        _loadingUI = UIMgr.Instance.GetUI<LoadingUI>();
        _preLoadDatas = new Dictionary<string, enPreLoadData>();
    }

    public void Add(string prefabPath, bool isUI = false)
    {
        var d = new enPreLoadData();
        d.prefabPath = prefabPath;
        d.isUI = isUI;
        if (!_preLoadDatas.ContainsKey(prefabPath))
            _preLoadDatas.Add(prefabPath , d);
    }

    public void Begin(Action func , float finishReady)
    {
        _currentCount = 0;
        _finishReadyMax = finishReady;
        _onFinish = func;
        TimerMgr.Instance.AddEvent(_Begin, 0.5f);
    }
    void _Begin()
    {
        _loadingUI.Begin(Load(), _OnLoadComplete);
    }
    IEnumerator Load()
    {
        if (_preLoadDatas.Count == 0)
        {
            _OnLoadUpdate(1);
            yield return null;
        }
        else
        {
            foreach (var d in _preLoadDatas.Values)
            {
                d.Load(ref _currentCount);
                _OnLoadUpdate((float)_currentCount / _preLoadDatas.Count);
                yield return null;
            }
        }
    }

    #region Event
    void _OnLoadUpdate(float percent)
    {
        _loadingUI.UpdateProgress(percent);
    }

    void _OnLoadComplete()
    {
        _isComplete = true;
        if (_onFinish != null)
            _onFinish();
        TimerMgr.Instance.AddEvent(_OnDestroy, _finishReadyMax);
    }

    void _OnDestroy()
    {
        Debug.Log("预加载完成，总资源数： " + _currentCount);
        UIMgr.Instance.RemoveUI(_loadingUI);
    }
    #endregion
    

}


