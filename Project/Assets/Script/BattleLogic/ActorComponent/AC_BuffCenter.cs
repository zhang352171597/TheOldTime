using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色技能效果接收中心
/// </summary>
public class AC_BuffCenter : ActorComponentBase
{
    static int BuffUID = 0;

    ActorHeadInfoCtr _headInfo;
    ActorHeadInfoCtr headInfo
    {
        get
        {
            if (_headInfo == null)
            {
                var ctr = actor.GetActorComponent<AC_HeadInfoCtr>();
                if (ctr != null)
                    _headInfo = ctr.headInfo;
            }
            return _headInfo;
        }
    }

    List<BuffSingle> _buffList;
    List<BuffSingle> _buffAddCache;
    List<BuffSingle> _buffRemoveCache;

    public override void OnAwake()
    {
        base.OnAwake();
        _buffList = new List<BuffSingle>();
        _buffAddCache = new List<BuffSingle>();
        _buffRemoveCache = new List<BuffSingle>();
    }

    public override void LogicUpdate(float dt)
    {
        base.LogicUpdate(dt);
        for(int i = 0 ; i < _buffList.Count; ++i)
        {
            _buffList[i].LogicUpdate(dt);
        }

        for(int i = 0 ; i < _buffAddCache.Count; ++i)
        {
            _buffList.Add(_buffAddCache[i]);
        }
        _buffAddCache.Clear();

        for(int i = 0 ; i < _buffRemoveCache.Count; ++i)
        {
            _buffRemoveCache[i].OnDestroy();
            _buffList.Remove(_buffRemoveCache[i]);
        }
        _buffRemoveCache.Clear();
    }

    /// <summary>
    /// 为此角色添加一个buff
    /// </summary>
    /// <param name="buffID">id</param>
    /// <param name="attacker">攻击者</param>
    /// <param name="addition">增减益</param>
    public void AddBuff(string buffID , ActorBase attacker , float addition = 0)
    {
        BuffUID++;
        var uid = "buff_" + BuffUID;
        var tempBuff = new BuffSingle(buffID, uid, attacker, this);
        if (tempBuff.isMomentBuff)
            tempBuff.OnDestroy();
        else
            _buffAddCache.Add(tempBuff);
    }

    /// <summary>
    /// 按需要移出某种buff
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="type"></param>
    public void RemoveBuff(BuffSingle buff, enBuffFunctionType type = enBuffFunctionType.基础)
    {
        if (buff == null)
        {
            for (int i = 0; i < _buffList.Count; ++i)
            {
                if (_buffList[i].filterType == type)
                    _buffRemoveCache.Add(_buffList[i]);
            }
        }
        else
            _buffRemoveCache.Add(buff);
    }

    BuffSingle getBuffByUID(string uid)
    {
        for(int i = 0 ; i < _buffList.Count; ++i)
        {
            if (_buffList[i].UID == uid)
                return _buffList[i];
        }
        return null;
    }

}

