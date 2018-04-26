using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色构造数据
/// </summary>
public class enActorConstructionData
{
    public enCamp camp;
    public enActorType type;
    public Vector3 bornPos;
    public string id;
}

/// <summary>
/// 角色数据层
/// </summary>
public class enActorAttributeData
{
    Dictionary<string, enActorAttribute> _dic;
    Dictionary<string, enActorAttribute> dic
    {
        get{
            if(_dic == null)
                _dic = new Dictionary<string,enActorAttribute>();
            return _dic;
        }
    }

    public void AddAttrbute(string key, int defaultValue, VoidDelegateIntInt callback , string maxLimitKey = "")
    {
        if (dic.ContainsKey(key))
        {
            Debug.LogWarning("the Actor Attribute of " + key + " is exist!");
            return;
        }
        var att = new enActorAttribute(key, defaultValue, maxLimitKey, this, callback);
        dic.Add(key, att);
    }

    public enActorAttribute GetAttribute(string key)
    {
        if (!dic.ContainsKey(key))
        {
            Debug.LogWarning("the Actor Attribute of " + key + " isn't exist!");
            AddAttrbute(key, 0, null);
        }
        return dic[key];
    }
    public void ChangeValue(string key , int v , enActorCoreAttChangeType changeType = enActorCoreAttChangeType.基础变更)
    {
        if (!dic.ContainsKey(key))
        {
            Debug.LogWarning("the Actor Attribute of " + key + " isn't exist!");
            return;
        }

        switch(changeType)
        {
            case enActorCoreAttChangeType.基础变更:
                dic[key].baseValue += v;
                break;
            case enActorCoreAttChangeType.增益变更:
                dic[key].gainValue += v;
                break;
            case enActorCoreAttChangeType.减益变更:
                dic[key].debuffValue += v;
                break;
        }
    }
    public void ResetBaseValue(string key, int v)
    {
        if (!dic.ContainsKey(key))
        {
            Debug.LogWarning("the Actor Attribute of " + key + " isn't exist!");
            return;
        }
        dic[key].baseValue = v;
    }
    public void Clear()
    {
        dic.Clear();
    }
}

/// <summary>
/// 角色数值层
/// </summary>
public class enActorAttribute
{
    VoidDelegateIntInt _onChange;

    string _key;
    public string key
    {
        get { return _key; }
    }
    
    public int Value
    {
        get 
        {
            return baseValue + gainValue + debuffValue; 
        }
    }

    int _baseValue;
    /// <summary>
    /// 基础值
    /// </summary>
    public int baseValue
    {
        get { return _baseValue + gainValue + debuffValue; }
        set 
        {
            var oldV = Value;
            if(_maxLimitKey.Length > 0)
            {
                var temp = _rootData.GetAttribute(_maxLimitKey).Value;
                _baseValue = value > temp ? temp : value;
            }
            else
                _baseValue = value;
            if (_onChange != null)
                _onChange(oldV , Value);
        }
    }
    int _gainValue;
    /// <summary>
    /// 全局增益值
    /// </summary>
    public int gainValue
    {
        get { return _gainValue; }
        set 
        {
            var oldV = Value;
            _gainValue = value;
            if (_onChange != null)
                _onChange(oldV , Value);
        }
    }
    

    int _debuffValue;
    /// <summary>
    /// 全局减益值
    /// </summary>
    public int debuffValue
    {
        get { return _debuffValue; }
        set 
        {
            var oldV = Value;
            _debuffValue = value;
            if (_onChange != null)
                _onChange(oldV , Value);
        }
    }
    string _maxLimitKey;
    public string maxLimitKey
    { get { return maxLimitKey; } }
    enActorAttributeData _rootData;
    /// <summary>
    /// 属性标识 默认值 最大值限制 变更回调
    /// </summary>
    /// <param name="k"></param>
    /// <param name="defaultV"></param>
    /// <param name="maxLimitKey"></param>
    /// <param name="onChangeFunc"></param>
    public enActorAttribute(string k, int defaultV, string maxLimitKey, enActorAttributeData rootData, VoidDelegateIntInt onChangeFunc)
    {
        _key = k;
        _baseValue = defaultV;
        _onChange = onChangeFunc;
        _maxLimitKey = maxLimitKey;
        _rootData = rootData;
    }
}