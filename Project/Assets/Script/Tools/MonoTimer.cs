using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 单位计时器
/// </summary>
public class TimerBase
{
    float _targetTime;
    Action _event;
    float _timecount;
    List<TimerBase> _removeCachePool;
    bool _runing;

    public TimerBase(Action action, float delay, ref List<TimerBase> cachePool)
    {
        _timecount = 0;
        _event = action;
        _targetTime = delay;
        _removeCachePool = cachePool;
        _runing = true;
    }

    public void UpdateLogic(float dt)
    {
        if (!_runing)
            return;

        _timecount += dt;
        if (_timecount >= _targetTime)
        {
            if (_event != null)
                _event();
            _removeCachePool.Add(this);
            _runing = false;
        }
    }
}

/// <summary>
/// 带计时器的脚本组件
/// </summary>
public class MonoTimer : MonoBehaviour {

    TimerCtr _timer;

    protected virtual void TimerBegin()
    {
        _timer = new TimerCtr();
    }

    protected virtual void TimerUpdate(float dt)
    {
        _timer.UpdateLogic(dt);
    }

    protected void AddTimerEvent(Action _event, float delay)
    {
        _timer.AddTimerEvent(_event, delay);
    }
}

public class TimerCtr
{
    List<TimerBase> _timers;
    List<TimerBase> _removeCache;

    public TimerCtr()
    {
        _timers = new List<TimerBase>();
        _removeCache = new List<TimerBase>();
    }

    public void UpdateLogic(float dt)
    {
        for (int i = 0; i < _timers.Count; ++i)
        {
            _timers[i].UpdateLogic(dt);
        }

        for (int i = 0; i < _removeCache.Count; ++i)
        {
            _timers.Remove(_removeCache[i]);
        }
        _removeCache.Clear();
    }

    public void AddTimerEvent(Action _event, float delay)
    {
        var timer = new TimerBase(_event, delay, ref _removeCache);
        _timers.Add(timer);
    }
}