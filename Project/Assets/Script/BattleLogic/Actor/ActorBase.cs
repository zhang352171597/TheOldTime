using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActorBase : MonoTimer
{

    #region 数据
    Vector3 _position;
    public Vector3 position
    {
        get { return _position; }
        set
        {
            _position = value;
            transform.position = value;
            Execute(enActorMessageType.onUpdatePos);
        }
    }

    //Temp
    public Vector3 headOffset
    {
        get { return Vector3.up * 3; }
    }

    public Vector3 bodyOffset
    {
        get { return Vector3.up * 1.5f; }
    }

    public Vector3 footOffset
    {
        get { return Vector3.zero; }
    }

    /// <summary>
    /// 头部状态信息位置
    /// </summary>
    public Vector3 headInfoPos
    {
        get
        {
            return position + headOffset;
        }
    }

	Vector3 _forward;
	public Vector3 forward{
		get{ return _forward;}
		set{
            transform.LookAt(position + value);
			_forward = value;
		}
	}

    /// <summary>
    /// 角色核心数值
    /// </summary>
    enActorAttributeData _coreAttribute;
    public enActorAttributeData coreAttribute
    {
        get
        {
            if (_coreAttribute == null)
                _coreAttribute = new enActorAttributeData();
            return _coreAttribute;
        }
    }

	public float speed
	{
		get
        {
            var att = coreAttribute.GetAttribute(ActorAttributeTag.speed);
            if (att == null)
                return 0;
            else
                return att.Value;
        }
	}

    enCamp _camp;
    public enCamp camp
    {
        get { return _camp; }
    }

    enActorType _type;
    public enActorType type
    {
        get { return _type; }
    }

    string _id;
    public string id
    { get { return _id; } }

    string _UID;
    public string UID
    {
        get { return _UID; }
    }

    bool _despawned;
    #endregion


    #region 控制
    List<ActorComponentBase> _componentPool;
    List<ActorComponentBase> _componentReadyPool;
    List<ActorComponentBase> _componentRemovePool;
    Dictionary<enActorMessageType, VoidDelegateMessage> _voidMessage;
    Dictionary<enActorMessageType, BoolDelegateMessage> _boolMessage;
    Dictionary<enActorMessageType, IntDelegateMessage> _intMessage;
    Dictionary<enActorMessageType, FloatDelegateMessage> _floatMessage;
    Dictionary<enActorMessageType, ObjectsDelegateMessage> _objsMessage;
    #endregion

    protected virtual void Awake()
    {
        _componentPool = new List<ActorComponentBase>();
        _componentReadyPool = new List<ActorComponentBase>();
        _componentRemovePool = new List<ActorComponentBase>();
        _voidMessage = new Dictionary<enActorMessageType, VoidDelegateMessage>();
        _boolMessage = new Dictionary<enActorMessageType, BoolDelegateMessage>();
        _intMessage = new Dictionary<enActorMessageType, IntDelegateMessage>();
        _floatMessage = new Dictionary<enActorMessageType, FloatDelegateMessage>();
        _objsMessage = new Dictionary<enActorMessageType, ObjectsDelegateMessage>();
        TimerBegin();
    }

    public virtual void Init(enActorConstructionData data)
    {
        _camp = data.camp;
        _type = data.type;
        _id = data.id;
		position = data.bornPos;
        forward = Vector3.right;
        _despawned = false;
        _UID = "Actor_" + ActorMgr.Instance.currentActorUID;
        AddActorComponent<AC_ModelCtr>();
        _InitAttribute();
    }
    protected virtual void _InitAttribute()
    {

    }
    public virtual void UpdateLogic(float dt)
    {
        _ComponentLogicUpdate(dt);
        TimerUpdate(dt);
    }
    public virtual void Despawn()
    {
        if (_despawned)
            return;
        Execute(enActorMessageType.tryDestroy);
        ClearAllComponent();
        ActorMgr.Instance.DespawnActor(this);
        _despawned = true;
    }
    #region ComponentMgr
    public T GetActorComponent<T>() where T : ActorComponentBase
    {
        var targetName = typeof(T).Name;
        for (int i = 0; i < _componentPool.Count; ++i)
        {
            var tempName = _componentPool[i].GetType().Name;
            if (tempName == targetName)
                return _componentPool[i] as T;
        }
        return AddActorComponent<T>();
    }

    public T AddActorComponent<T>() where T : ActorComponentBase
    {
        var t = Activator.CreateInstance<T>();
        t.actor = this;
        t.OnAwake();
        _componentReadyPool.Add(t);
        return t;
    }

    public void RemoveActorComponent<T>() where T : ActorComponentBase
    {
        var t = Activator.CreateInstance<T>();
        _componentRemovePool.Add(t);
    }

    public void ClearAllComponent()
    {
        for(int i = 0 ; i < _componentPool.Count; ++i)
        {
            _componentRemovePool.Add(_componentPool[i]);
        }
    }

    void _ComponentLogicUpdate(float dt)
    {
        for (int i = 0; i < _componentReadyPool.Count; ++i)
        {
            _componentPool.Add(_componentReadyPool[i]);
            _componentReadyPool[i].OnStart();
        }
        if (_componentReadyPool.Count > 0)
            _componentReadyPool.Clear();

        for (int i = 0; i < _componentPool.Count; ++i)
        {
            _componentPool[i].LogicUpdate(dt);
        }
        for (int i = 0; i < _componentRemovePool.Count; ++i)
        {
            _componentPool.Remove(_componentRemovePool[i]);
            _componentRemovePool[i].OnDestroy();
        }
        if (_componentRemovePool.Count > 0)
            _componentRemovePool.Clear();
    }
    #endregion

    #region Callback
    protected void _OnHpChange(int oldV , int newV)
    {
        Execute(enActorMessageType.onUpdateHp, oldV, newV);
    }
    protected void _OnHpMaxChange(int oldV, int newV)
    {
        Execute(enActorMessageType.onUpdatHPMax, oldV, newV);
    }
    protected void OnShielChange(int oldV, int newV)
    {
        Execute(enActorMessageType.onUpdateShield, oldV, newV);
    }
    #endregion
    #region MessageCenter
    public void Bind(enActorMessageType type, VoidDelegateMessage func)
    {
        if (_voidMessage.ContainsKey(type))
            _voidMessage[type] += func;
        else
            _voidMessage.Add(type, func);
    }
    public void UnBind(enActorMessageType type, VoidDelegateMessage func)
    {
        if (_voidMessage.ContainsKey(type))
            _voidMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }
    public void Execute(enActorMessageType type, params System.Object[] objs)
    {
        if (_voidMessage.ContainsKey(type))
            _voidMessage[type](objs);
    }
    public void BindBool(enActorMessageType type, BoolDelegateMessage func)
    {
        if (_boolMessage.ContainsKey(type))
            _boolMessage[type] += func;
        else
            _boolMessage.Add(type, func);
    }

    public void UnBindBool(enActorMessageType type, BoolDelegateMessage func)
    {
        if (_boolMessage.ContainsKey(type))
            _boolMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }
    public bool ExecuteBool(enActorMessageType type, params System.Object[] objs)
    {
        if (_boolMessage.ContainsKey(type))
            return _boolMessage[type](objs);
        return false;
    }
    public void BindInt(enActorMessageType type, IntDelegateMessage func)
    {
        if (_intMessage.ContainsKey(type))
            _intMessage[type] += func;
        else
            _intMessage.Add(type, func);
    }
    public void UnBindInt(enActorMessageType type, IntDelegateMessage func)
    {
        if (_intMessage.ContainsKey(type))
            _intMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public int ExecuteInt(enActorMessageType type, params System.Object[] objs)
    {
        if (_intMessage.ContainsKey(type))
            return _intMessage[type](objs);
        return 0;
    }
    public void BindFloat(enActorMessageType type, FloatDelegateMessage func)
    {
        if (_floatMessage.ContainsKey(type))
            _floatMessage[type] += func;
        else
            _floatMessage.Add(type, func);
    }

    public void UnBindFloat(enActorMessageType type, FloatDelegateMessage func)
    {
        if (_floatMessage.ContainsKey(type))
            _floatMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public float ExecuteFloat(enActorMessageType type, params System.Object[] objs)
    {
        if (_floatMessage.ContainsKey(type))
            return _floatMessage[type](objs);
        return 0;
    }

    public void BindObjs(enActorMessageType type, ObjectsDelegateMessage func)
    {
        if (_objsMessage.ContainsKey(type))
            _objsMessage[type] += func;
        else
            _objsMessage.Add(type, func);
    }

    public void UnBindObjs(enActorMessageType type, ObjectsDelegateMessage func)
    {
        if (_objsMessage.ContainsKey(type))
            _objsMessage[type] -= func;
        else
            Debug.LogWarning("want remove a message func is not exist!");
    }

    public System.Object[] ExecuteObjs(enActorMessageType type, params System.Object[] objs)
    {
        if (_objsMessage.ContainsKey(type))
            return _objsMessage[type](objs);
        return new System.Object[0];
    }
    #endregion
}
