using UnityEngine;
using System.Collections;

/// <summary>
/// 角色状态控制中心
/// </summary>
public class AC_StateCtrBase : ActorComponentBase
{
    TimerCtr _timer;
    enActorState _state = enActorState.idle;

    public override void OnAwake()
    {
        _timer = new TimerCtr();
        actor.Bind(enActorMessageType.tryIdle, _TryIdle);
        actor.Bind(enActorMessageType.tryMove, _TryMove);
        actor.Bind(enActorMessageType.tryDead, _TryDie);
        actor.Bind(enActorMessageType.onDead, _OnDead);
        actor.BindInt(enActorMessageType.getCurrentState, _GetCurrentState);
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnDestroy()
    {
        actor.UnBind(enActorMessageType.tryIdle, _TryIdle);
        actor.UnBind(enActorMessageType.tryMove, _TryMove);
        actor.UnBind(enActorMessageType.tryDead, _TryDie);
        actor.UnBind(enActorMessageType.onDead, _OnDead);
        actor.UnBindInt(enActorMessageType.getCurrentState, _GetCurrentState);
    }

    public override void LogicUpdate(float dt)
    {
        _timer.UpdateLogic(dt);
    }
    #region Event
    void _TryIdle(params System.Object[] objs)
    {
        if (_state != enActorState.idle)
        {
            _state = enActorState.idle;
            actor.Execute(enActorMessageType.onIdle);
        }
    }
    void _TryMove(params System.Object[] objs)
    {
        if (_state != enActorState.move)
        {
            _state = enActorState.move;
            actor.Execute(enActorMessageType.onMove);
        }
    }
    void _TryDie(params System.Object[] objs)
    {
        ///此处处理一些个别要死不死的特殊逻辑
        if (_state != enActorState.dead)
        {
            _state = enActorState.dead;
            actor.Execute(enActorMessageType.onDead);
        }
    }
    void _OnDead(params System.Object[] objs)
    {
        _timer.AddTimerEvent(_OnDieFinish, 3);
    }
    void _OnDieFinish()
    {
        actor.Execute(enActorMessageType.tryDropItem);
        actor.Despawn();
    }
    int _GetCurrentState(object[] _objs)
    {
        return (int)_state;
    }
    #endregion
}
