using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 消息中心
/// </summary>
public class MessageCenter : ModuleComponent<MessageCenter> 
{
    Dictionary<enGameMessageType, VoidDelegateMessage> _voidMessage;
    Dictionary<enGameMessageType, BoolDelegateMessage> _boolMessage;
    Dictionary<enGameMessageType, IntDelegateMessage> _intMessage;
    Dictionary<enGameMessageType, FloatDelegateMessage> _floatMessage;
    Dictionary<enGameMessageType, ObjectsDelegateMessage> _objsMessage;

    void Awake()
    {
        _voidMessage = new Dictionary<enGameMessageType, VoidDelegateMessage>();
        _boolMessage = new Dictionary<enGameMessageType, BoolDelegateMessage>();
        _intMessage = new Dictionary<enGameMessageType, IntDelegateMessage>();
        _floatMessage = new Dictionary<enGameMessageType, FloatDelegateMessage>();
        _objsMessage = new Dictionary<enGameMessageType, ObjectsDelegateMessage>();
    }
    
    public void Clear()
    {
        _voidMessage.Clear();
        _boolMessage.Clear();
        _intMessage.Clear();
        _floatMessage.Clear();
        _objsMessage.Clear();
    }

    public void Bind(enGameMessageType type, VoidDelegateMessage func)
    {
        if (_voidMessage.ContainsKey(type))
            _voidMessage[type] += func;
        else
            _voidMessage.Add(type, func);
    }

    public void UnBind(enGameMessageType type, VoidDelegateMessage func)
    {
        if (_voidMessage.ContainsKey(type))
            _voidMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public void Execute(enGameMessageType type, params System.Object[] objs)
    {
        if (_voidMessage.ContainsKey(type))
            _voidMessage[type](objs);
    }

    public void BindBool(enGameMessageType type, BoolDelegateMessage func)
    {
        if (_boolMessage.ContainsKey(type))
            _boolMessage[type] += func;
        else
            _boolMessage.Add(type, func);
    }

    public void UnBindBool(enGameMessageType type, BoolDelegateMessage func)
    {
        if (_boolMessage.ContainsKey(type))
            _boolMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public bool ExecuteBool(enGameMessageType type, params System.Object[] objs)
    {
        if (_boolMessage.ContainsKey(type))
            return _boolMessage[type](objs);
        return false;
    }
    public void BindInt(enGameMessageType type, IntDelegateMessage func)
    {
        if (_intMessage.ContainsKey(type))
            _intMessage[type] += func;
        else
            _intMessage.Add(type, func);
    }

    public void UnBindInt(enGameMessageType type, IntDelegateMessage func)
    {
        if (_intMessage.ContainsKey(type))
            _intMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public int ExecuteInt(enGameMessageType type, params System.Object[] objs)
    {
        if (_intMessage.ContainsKey(type))
            return _intMessage[type](objs);
        return 0;
    }
    public void BindFloat(enGameMessageType type, FloatDelegateMessage func)
    {
        if (_floatMessage.ContainsKey(type))
            _floatMessage[type] += func;
        else
            _floatMessage.Add(type, func);
    }

    public void UnBindFloat(enGameMessageType type, FloatDelegateMessage func)
    {
        if (_floatMessage.ContainsKey(type))
            _floatMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public float ExecuteFloat(enGameMessageType type, params System.Object[] objs)
    {
        if (_floatMessage.ContainsKey(type))
            return _floatMessage[type](objs);
        return 0;
    }

    public void BindObjs(enGameMessageType type, ObjectsDelegateMessage func)
    {
        if (_objsMessage.ContainsKey(type))
            _objsMessage[type] += func;
        else
            _objsMessage.Add(type, func);
    }

    public void UnBindObjs(enGameMessageType type, ObjectsDelegateMessage func)
    {
        if (_objsMessage.ContainsKey(type))
            _objsMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public System.Object[] ExecuteObjs(enGameMessageType type, params System.Object[] objs)
    {
        if (_objsMessage.ContainsKey(type))
            return _objsMessage[type](objs);
        return new System.Object[0];
    }
}
