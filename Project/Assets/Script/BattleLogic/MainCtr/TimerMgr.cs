using UnityEngine;
using System.Collections;

public class TimerMgr : ModuleComponent<TimerMgr> 
{
    TimerCtr _timer;
    TimerCtr timer
    {
        get
        {
            if (_timer == null)
                _timer = new TimerCtr();
            return _timer;
        }
    }

    public void Load()
    {

    }

    public void Begin()
    {

    }

    public void UpdateLogic(float dt)
    {
        timer.UpdateLogic(Time.deltaTime);
    }
	
    public void AddEvent(System.Action action , float delay)
    {
        timer.AddTimerEvent(action, delay);
    }

    public void Clear()
    {
        _timer = null;
    }
}
