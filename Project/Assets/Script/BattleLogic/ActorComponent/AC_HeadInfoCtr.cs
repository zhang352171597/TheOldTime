using UnityEngine;
using System.Collections;

/// <summary>
/// 角色头部信息显示控制器
/// </summary>
public class AC_HeadInfoCtr : ActorComponentBase 
{
    ActorHeadInfoCtr _uiCtr;
    public ActorHeadInfoCtr headInfo
    {
        get { return _uiCtr; }
    }
    public override void OnAwake()
    {
        base.OnAwake();
        actor.Bind(enActorMessageType.onUpdatePos, _onUpdatePos);
        actor.Bind(enActorMessageType.onUpdateHp, _onUpdateHp);
        actor.Bind(enActorMessageType.onUpdatHPMax, _onUpdatHPMax);
        actor.Bind(enActorMessageType.onUpdateShield, _onUpdateShield);
        actor.Bind(enActorMessageType.onDead, _onHindHeadInfo);
        MessageCenter.Instance.Bind(enGameMessageType.worldCameraUpdate, _onUpdatePos);
    }
    public override void OnStart()
    {
        base.OnStart();
        _uiCtr = UIMgr.Instance.GetUI<BattleMainUI>().headInfoModule.getHeadInfo(actor);
        _uiCtr.UpdateHp(actor.coreAttribute.GetAttribute(ActorAttributeTag.hp).Value, actor.coreAttribute.GetAttribute(ActorAttributeTag.maxHp).Value);
    }
    public override void OnDestroy()
    {
        actor.UnBind(enActorMessageType.onUpdatePos, _onUpdatePos);
        actor.UnBind(enActorMessageType.onUpdateHp, _onUpdateHp);
        actor.UnBind(enActorMessageType.onUpdatHPMax, _onUpdatHPMax);
        actor.UnBind(enActorMessageType.onUpdateShield, _onUpdateShield);
        actor.UnBind(enActorMessageType.onDead, _onHindHeadInfo);
		_onHindHeadInfo ();
        MessageCenter.Instance.UnBind(enGameMessageType.worldCameraUpdate, _onUpdatePos);
    }
    void _onUpdatePos(params System.Object[] objs)
    {
        if (_uiCtr == null)
            return;

        _uiCtr.UpdatePos(actor.headInfoPos);
    }
    void _onUpdateHp(params System.Object[] objs)
    {
        var oldV = (int)objs[0];
        var newV = (int)objs[1];
        var changeV = newV - oldV;
        _uiCtr.UpdateHp(newV, actor.coreAttribute.GetAttribute(ActorAttributeTag.maxHp).Value);
        if (changeV != 0)
            _uiCtr.ShowNumber(changeV);
        if (newV <= 0)
            actor.Execute(enActorMessageType.tryDead);
    }
    void _onUpdatHPMax(params System.Object[] objs)
    {
        var oldV = (int)objs[0];
        var newV = (int)objs[1];
        var changeV = newV - oldV;
        _uiCtr.UpdateHp(actor.coreAttribute.GetAttribute(ActorAttributeTag.hp).Value, actor.coreAttribute.GetAttribute(ActorAttributeTag.maxHp).Value);
    }
    void _onUpdateShield(params System.Object[] objs)
    {
        var oldV = (int)objs[0];
        var newV = (int)objs[1];
        var changeV = newV - oldV;
        _uiCtr.UpdateShield((float)newV / actor.coreAttribute.GetAttribute(ActorAttributeTag.maxHp).Value);
        if (changeV != 0)
            _uiCtr.ShowNumber(changeV);
    }
    void _onHindHeadInfo(params System.Object[] objs)
    {
        UIMgr.Instance.GetUI<BattleMainUI>().headInfoModule.Despawn(actor.UID);
    }
}
