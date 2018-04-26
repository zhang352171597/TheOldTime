using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JSON;

/// <summary>
/// 技能释放数据
/// </summary>
public class enSkillReleaseData
{
    enSkillReleaseState[] stateDatas;
    public enSkillReleaseState currentStateData
    {
        get
        {
            if (_currentIndex < stateDatas.Length)
                return stateDatas[_currentIndex];
            return null;
        }
    }

    /// <summary>
    /// 上一个释放状态
    /// </summary>
    public enSkillReleaseState lastCurrentState
    {
        get { 
            if(stateDatas.Length > 0)
            {
                var lastIndex = _currentIndex - 1;
                if (lastIndex >= 0 && lastIndex < stateDatas.Length - 1)
                    return stateDatas[lastIndex];
            }
            return null;
        }
    }

    int _currentIndex;
    DelegateOfString _onStateChange;
    DelegateOfReleaseState _onRelease;
    ActorBase _lastHittedActor;
    public ActorBase lastHittedActor
    {
        get { return _lastHittedActor; }
        set { _lastHittedActor = value; }
    }
    string _id;
    public string id
    { get { return _id; } }

    public enSkillReleaseData(string id)
    {
        //TODO:解析释放片段
        if(!JSONSkillDic.dicById.ContainsKey(id))
            Debug.LogError("skill is not exist, id : " + id);

        _id = id;
        var skillNode = JSONSkillDic.dicById[id];
        var states = JSONNode.Parse(skillNode[SkillTag.states]);
        stateDatas = new enSkillReleaseState[states.Count];
        for (int i = 0; i < states.Count; ++i )
        {
            stateDatas[i] = new enSkillReleaseState();
            stateDatas[i].singleData = new enSkillSingleData(states[i]);
            stateDatas[i].releaseData = this;
        }
    }

    public void SetReleaseBind(DelegateOfReleaseState releaseBack)
    {
        _onRelease = releaseBack;
    }

    public void Bind(DelegateOfFloat cdChangeBack, DelegateOfFloat onBeforeTimeChange, DelegateOfFloat onAfterTimeChange ,DelegateOfString stateChangeBack)
    {
        _onStateChange = stateChangeBack;
        for(int i = 0; i < stateDatas.Length; ++i)
        {
            stateDatas[i].Bind(cdChangeBack, onBeforeTimeChange, onAfterTimeChange, NextState);
        }
        Reset();
    }

    public void UpdateLogic(float dt)
    {
        if (currentStateData != null)
            currentStateData.UpdateLogic(dt);
    }

    public enSkillReleaseState Release()
    {
        var stateData = currentStateData;
        if (_onRelease != null)
            _onRelease(stateData);
        stateData.Release();
        return stateData;
    }

    public bool isFirstState(enSkillReleaseState stateData)
    {
        return stateDatas[0] == stateData;
    }

    void Reset()
    {
        for(int i = 0 ; i < stateDatas.Length; ++i)
        {
            stateDatas[i].isActive = false;
        }
        _currentIndex = 0;
        currentStateData.ChangeActive(true);
    }

    void NextState(bool isFinal)
    {
        _currentIndex = isFinal ? 999 : _currentIndex + 1;
        if (currentStateData == null)
        {
            _currentIndex = 0;
            currentStateData.setCDPercent(0);
            currentStateData.ChangeActive();
            _ClearAllHited();
        }
        else
            currentStateData.ChangeActive(true);

        if (stateDatas.Length > 1 && _onStateChange != null)
            _onStateChange(currentStateData.singleData.icon);
    }

    void _ClearAllHited()
    {
        for (int i = 0; i < stateDatas.Length; ++i)
        {
            stateDatas[i].hitedActors.Clear();
        }
    }

}

/// <summary>
/// 技能释放状态
/// </summary>
public class enSkillReleaseState
{

    /// <summary>
    /// 状态操作类型
    /// </summary>
    public enum enStateOperation
    {
        /// <summary>
        /// 结束
        /// </summary>
        finish,
        /// <summary>
        /// 下个状态
        /// </summary>
        nextState,
    }

    enSkillReleaseData _releaseData;
    public enSkillReleaseData releaseData
    {
        get { return _releaseData; }
        set { _releaseData = value; }
    }

    public enSkillSingleData singleData;

    bool _isActive;
    /// <summary>
    /// 是否处于激活状态
    /// </summary>
    public bool isActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    public bool OnCold
    {
        get{return _currentColdTime > 0;}
    }

    float _currentReady;
    float _currentBeforeTime;
    float _currentAfterTime;
    float _currentColdTime;
    DelegateOfFloat _onCDChange;
    DelegateOfFloat _onBeforeTimeChange;
    DelegateOfFloat _onAfterTimeChange;
    DelegateOfVoidWithBool _onNextState;

    List<ActorBase> _hitedActors;
    /// <summary>
    /// 将该状态下释放的技能所命中的目标缓存
    /// </summary>
    public List<ActorBase> hitedActors
    {
        get 
        {
            if (_hitedActors == null)
                _hitedActors = new List<ActorBase>();
            return _hitedActors; 
        }
    }
    public void Bind(DelegateOfFloat onCDChange, DelegateOfFloat onBeforeTimeChange, DelegateOfFloat onAfterTimeChange, DelegateOfVoidWithBool onNextState)
    {
        _onCDChange = onCDChange;
        _onBeforeTimeChange = onBeforeTimeChange;
        _onAfterTimeChange = onAfterTimeChange;
        _onNextState = onNextState;
    }

    public void setCDPercent(float percent)
    {
        _currentReady = 0;
        _currentBeforeTime = 0;
        _currentAfterTime = 0;
        if (_onBeforeTimeChange != null)
            _onBeforeTimeChange(_currentBeforeTime);

        _currentColdTime = (1 - percent) * singleData.coldTime;
        if (_currentColdTime < 0)
            _currentColdTime = 0;

        if (_onCDChange != null)
            _onCDChange(_currentColdTime / singleData.coldTime);
    }

    public void UpdateLogic(float dt)
    {
        if (!isActive)
            return;

        if (_currentReady > 0 && singleData.readyTime > 0)
        {
            _currentReady -= dt;
            if (_onCDChange != null)
                _onCDChange(_currentReady / singleData.readyTime);
        }
        else
        {
            if(_currentAfterTime > 0 && singleData.afterTime > 0)
            {
                _currentAfterTime -= dt;
                if (_onAfterTimeChange != null)
                    _onAfterTimeChange(_currentAfterTime);
                if (_currentAfterTime <= 0)
                    _FinalAllState();
            }
            else if (_currentColdTime > 0 && singleData.coldTime > 0)
            {
                _currentColdTime -= dt;
                if (_onCDChange != null)
                    _onCDChange(_currentColdTime / singleData.coldTime);
            }
            else
            {
                if (_currentBeforeTime > 0 && singleData.beforeTime > 0)
                {
                    _currentBeforeTime -= dt;
                    if (_onBeforeTimeChange != null)
                        _onBeforeTimeChange(_currentBeforeTime);
                    if (_currentBeforeTime <= 0)
                        _FinalAllState();
                }
            }
        }
    }

    public void ChangeActive(bool isReady = false)
    {
        isActive = true;
        _currentReady = isReady ? singleData.readyTime : 0;
        _currentBeforeTime = singleData.beforeTime;
    }

    public void Release()
    {
        ///存在释放后摇等待时间 则在当前状态待定
        if (singleData.afterTime > 0)
            _currentAfterTime = singleData.afterTime;
        else //否则切换至下一状态/CD
            _NextState();
    }

    public void OperationByType(enStateOperation type)
    {
        ///未激活 或者 已处于冷却状态  则不可以由外界干扰
        /*if (!isActive || OnCold)
            return;*/

        switch(type)
        {
            case enStateOperation.finish:
                {
                    _FinalAllState();
                }
                break;
            case enStateOperation.nextState:
                    _NextState();
                break;
        }
    }
    public void AddHiedActor(ActorBase actor)
    {
        releaseData.lastHittedActor = actor;
        hitedActors.Add(actor);
    }
    void _NextState()
    {
        _currentColdTime = singleData.coldTime;
        _onNextState(false);
        if (!releaseData.isFirstState(this))
            isActive = false;
    }

    /// <summary>
    /// 结束所有操作等待--即直接进入cd状态
    /// </summary>
    void _FinalAllState()
    {
        isActive = false;
        _onNextState(true);
    }
}

/// <summary>
/// 技能释放单元
/// </summary>
public class enSkillSingleData
{
    /// <summary>
    /// 该片段技能表现预制
    /// </summary>
    public string prefabs;
    /// <summary>
    /// 该片段图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 剩余解冻时间 0时  冷却完毕
    /// </summary>
    public float coldTime;
    /// <summary>
    /// 当技能冷却完毕之后 准备时间结束才可以释放
    /// </summary>
    public float readyTime;
    /// <summary>
    /// 释放前段计时 --即技能可以释放时计时 计时结束技能将进入CD
    /// </summary>
    public float beforeTime = -1;
    /// <summary>
    /// 释放后段计时 --即技能释放完开始计时 不直接切换下一状态 计时结束技能将进入CD 
    /// </summary>
    public float afterTime = -1;
    /// <summary>
    /// 生成位置偏移
    /// </summary>
    public float bornPosOffset;
    /// <summary>
    /// 详情
    /// </summary>
    public string info;
    public bool isDataChangeByEffect;
    public enSkillSingleData(JSONNode node)
    {
        prefabs = node[SkillTag.prefabName];
        icon = node[JSONTag.icon];
        info = node[JSONTag.info];
        coldTime = node[SkillTag.coldTime].AsFloat;
        readyTime = node[SkillTag.readyTime].AsFloat;
        beforeTime = node[SkillTag.beforeTime].AsFloat;
        afterTime = node[SkillTag.afterTime].AsFloat;
        bornPosOffset = node[SkillTag.bornPosOffset].AsFloat;
    }
}



/// <summary>
/// 技能构造数据
/// </summary>
public class enSkillConstructionData
{
    public ActorBase attacker;
    /// <summary>
    /// 当前所要创建的目标技能隶属于哪个技能状态
    /// </summary>
    public enSkillReleaseState belongStateData;
}


/// <summary>
/// 技能字典类
/// </summary>
public class JSONSkillDic
{
    static Dictionary<string, JSONNode> _dicById;

    ~JSONSkillDic()
    {
        _dicById = null;
    }

    public static Dictionary<string, JSONNode> dicById
    {
        get
        {
            if (_dicById == null)
            {
                _dicById = new Dictionary<string, JSONNode>();
                var nodes = JSONSkill.jsonNode[JSONTag.root];
                for (int i = 0; i < nodes.Count; ++i)
                {
                    var id = nodes[i][JSONTag.id];
                    _dicById.Add(id, nodes[i]);
                }
            }
            return _dicById;
        }
    }

    public static void Clear()
    {
        _dicById = null;
    }
}

/// <summary>
/// 技能配表类
/// </summary>
public class JSONSkill
{
    static JSONNode _json;
    public static void Clear()
    {
        _json = null;
    }
    public static JSONNode jsonNode
    {
        get
        {
            if (_json == null)
            {
                _json = JsonRead.Read(SkillTag.JSONName);
            }
            return _json;
        }
    }
}