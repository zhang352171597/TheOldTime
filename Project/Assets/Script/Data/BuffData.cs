using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;
using DG.Tweening;
#region Buff逻辑类
public class BuffSingle
{
    AC_BuffCenter _center;
    ActorBase _attacker;
    string _buffID;
    enBuffFunctionType _filterType;
    public enBuffFunctionType filterType
    {
        get { return _filterType; }
    }
    string _uID;
    public string UID
    {
        get { return _uID; }
    }
    /// <summary>
    /// 是否是瞬间buff  即不需要缓存在buff中心跑逻辑
    /// </summary>
    bool _isMomentBuff;
    public bool isMomentBuff
    {
        get { return _isMomentBuff; }
    }
    /// <summary>
    /// 作用片段
    /// </summary>
    List<enBuffClipData> _clipList;
    /// <summary>
    /// buff逻辑类型 瞬间  持续  状态
    /// </summary>
    enBuffType _type;
    /// <summary>
    /// 持续时间
    /// </summary>
    float _duration;
    float _durationCount;
    /// <summary>
    /// 频率
    /// </summary>
    float _frequency;
    float _frequencyCount;
    public BuffSingle(string buffID, string uID, ActorBase attacker, AC_BuffCenter center)
    {
        _buffID = buffID;
        _uID = uID;
        _attacker = attacker;
        _center = center;
        _clipList = new List<enBuffClipData>();
        //TODO:解析buffNode
        if (!JSONBuffDic.dicById.ContainsKey(buffID))
        {
            DeBugger.LogWarning("the buff isn't exist. id is  " + _buffID);
            _isMomentBuff = true;
            return;
        }
        else
        {
            var node = JSONBuffDic.dicById[buffID];
            var name = node[JSONTag.name];
            _type = (enBuffType)node[BuffTag.type].AsInt;
            _isMomentBuff = _type == enBuffType.瞬间;
            _filterType = (enBuffFunctionType)node[BuffTag.functionType].AsInt;
            _duration = float.Parse(node[BuffTag.duration]);
            _durationCount = _duration;
            _frequency = float.Parse(node[BuffTag.frequency]);
            _frequencyCount = _frequency;
            var clipNodes = JSONNode.Parse(node[BuffTag.buffClips]);
            for (int i = 0; i < clipNodes.Count; ++i)
            {
                _clipList.Add(new enBuffClipData(clipNodes[i]));
                _clipList[i].Work(_type, _attacker, _center);
            }
        }
    }
    public virtual void OnDestroy()
    {
        for (int i = 0; i < _clipList.Count; ++i)
        {
            _clipList[i].OnRemove();
        }
    }
    public virtual void LogicUpdate(float dt)
    {
        if (_type == enBuffType.持续 && _frequencyCount > 0)
        {
            _frequencyCount -= dt;
            if(_frequencyCount <= 0)
            {
                _Work();
                _frequencyCount = _frequency;
            }
        }
        if(_type != enBuffType.瞬间)
        {
            _durationCount -= dt;
            if (_durationCount <= 0)
                _center.RemoveBuff(this);
        }
    }
    void _Work()
    {
        for(int i = 0 ; i < _clipList.Count; ++i)
        {
            _clipList[i].Work(_type , _attacker, _center);
        }
    }
    string getString(JSONNode node, string key)
    {
        var result = node[key] != null ? node[key].ToString() : "??";
        return result;
    }
    int getInt(JSONNode node, string key)
    {
        return node[key].AsInt;
    }
    float getFloat(JSONNode node, string key)
    {
        return node[key].AsFloat;
    }
    bool getBool(JSONNode node, string key)
    {
        return node[key].AsInt == 1;
    }
}
#endregion

#region buff作用片段数据
public class enBuffClipData
{
    /// <summary>
    /// 取值参照方
    /// </summary>
    public enBuffReferType referType;
    /// <summary>
    /// 取值参照标识
    /// </summary>
    public enBuffAttTargetTag referTagType;
    /// <summary>
    /// 作用方
    /// </summary>
    public enBuffReferType workType;
    /// <summary>
    /// 作用数值标识
    /// </summary>
    public enBuffAttTargetTag workTagType;
    /// <summary>
    /// 核心数值类型
    /// </summary>
    public enBuffCoreValueType coreValueType;
    /// <summary>
    /// 核心作用值
    /// </summary>
    public float coreValue;
    /// <summary>
    /// 最小有效值
    /// </summary>
    public int minValue;
    /// <summary>
    /// 最大有效值
    /// </summary>
    public int maxValue;
    /// <summary>
    /// 实际作用值
    /// </summary>
    public int _finalWorkValue;
    /// <summary>
    /// 逻辑类型
    /// </summary>
    enBuffType _type;
    /// <summary>
    /// 作用数据
    /// </summary>
    enActorAttribute _workTarget;
    public enBuffClipData(JSONNode node)
    {
        referType = (enBuffReferType)node[BuffTag.referType].AsInt;
        referTagType = (enBuffAttTargetTag)node[BuffTag.referTagType].AsInt;
        workType = (enBuffReferType)node[BuffTag.workType].AsInt;
        workTagType = (enBuffAttTargetTag)node[BuffTag.workTagType].AsInt;
        coreValueType = (enBuffCoreValueType)node[BuffTag.coreValueType].AsInt;
        coreValue = node[BuffTag.coreValue].AsFloat;
        minValue = node[BuffTag.minValue].AsInt;
        maxValue = node[BuffTag.maxValue].AsInt;
    }
    /// <summary>
    /// 生效
    /// </summary>
    public void Work(enBuffType buffType , ActorBase attacker, AC_BuffCenter center)
    {
        _type = buffType;
        ///取值方
        var referActor = referType == enBuffReferType.施法者 ? attacker : center.actor;
        ///作用方
        var workActor = workType == enBuffReferType.施法者 ? attacker : center.actor;
        _workTarget = GetAttDataByType(workActor.coreAttribute, workTagType);
        if (coreValueType == enBuffCoreValueType.固定值)
        {
            _finalWorkValue = (int)coreValue;
            _workTarget.baseValue += _finalWorkValue;
        }
        else if (coreValueType == enBuffCoreValueType.百分比)
        {
            ///取值方数据中心
            var referValue = getReferValue(referActor.coreAttribute, referTagType);
            _finalWorkValue = (int)(referValue * coreValue / 100);
            _finalWorkValue = minValue != 0 && _finalWorkValue < minValue ? minValue : _finalWorkValue;
            _finalWorkValue = maxValue != 0 && _finalWorkValue > maxValue ? maxValue : _finalWorkValue;
            _workTarget.baseValue += _finalWorkValue;
        }
		//Temp
		if (_type == enBuffType.状态) {
			//临时击飞
			//TODO:限制AI
			center.actor.transform.DOJump (center.actor.transform.position, 2, 1, 0.5f);
		}
    }
    /// <summary>
    /// 当持续时间结束或者强制移出的时候
    /// </summary>
    public void OnRemove()
    {
        if (_type == enBuffType.状态 && _workTarget != null)
        {
            _workTarget.baseValue -= _finalWorkValue;
        }
    }
    /// <summary>
    /// 通过类型获取属性数值
    /// </summary>
    /// <param name="coreAttData"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    int getReferValue(enActorAttributeData coreAttData, enBuffAttTargetTag type)
    {
        if (type == enBuffAttTargetTag.已损血量)
        {
            var v1 = GetAttDataByType(coreAttData, enBuffAttTargetTag.最大血量).Value;
            var v2 = GetAttDataByType(coreAttData, enBuffAttTargetTag.当前血量).Value;
            return v1 - v2;
        }
        else
            return GetAttDataByType(coreAttData, type).Value;
    }
    /// <summary>
    /// 通过类型获取属性对象
    /// </summary>
    /// <returns></returns>
    enActorAttribute GetAttDataByType(enActorAttributeData coreAttData, enBuffAttTargetTag type)
    {
        if (type == enBuffAttTargetTag.当前血量)
            return coreAttData.GetAttribute(ActorAttributeTag.hp);
        else if (type == enBuffAttTargetTag.最大血量)
            return coreAttData.GetAttribute(ActorAttributeTag.maxHp);
        else if (type == enBuffAttTargetTag.物攻)
            return coreAttData.GetAttribute(ActorAttributeTag.ad);
        else if (type == enBuffAttTargetTag.物防)
            return coreAttData.GetAttribute(ActorAttributeTag.df);
        else if (type == enBuffAttTargetTag.魔攻)
            return coreAttData.GetAttribute(ActorAttributeTag.ap);
        else if (type == enBuffAttTargetTag.魔防)
            return coreAttData.GetAttribute(ActorAttributeTag.pf);
        else if (type == enBuffAttTargetTag.速度)
            return coreAttData.GetAttribute(ActorAttributeTag.speed);
        else
        {
            EditorDebug.LogError("未知BUFF作用数值类型: " + type);
            return null;
        }
    }
}
#endregion

#region Buff静态配置

/// <summary>
/// 技能配表类
/// </summary>
public class JSONBuff
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
                _json = JsonRead.Read(BuffTag.JSONName);
            }
            return _json;
        }
    }
}


/// <summary>
/// 技能字典类
/// </summary>
public class JSONBuffDic
{
    static Dictionary<string, JSONNode> _dicById;

    ~JSONBuffDic()
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
                var nodes = JSONBuff.jsonNode[JSONTag.root];
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
#endregion

