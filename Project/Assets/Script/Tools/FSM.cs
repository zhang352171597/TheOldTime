using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM
{
    FSMState _lastState;
    DelegateOfVoid _lastExit;
    FSMState _currentState;
    List<FSMState> _stateList = new List<FSMState>();

    /// <summary>
    /// 当前状态进行时间
    /// </summary>
    float _currentStateTime;
    public float currentStateTime
    {
        get { return _currentStateTime; }
    }

    public void AddState(FSMState state, DelegateOfVoid enterFunc, DelegateOfUpdate updateFunc, DelegateOfVoid exitFunc)
    {
        if (_stateList.Contains(state))
            return;

        _stateList.Add(state);
        state.Begin(enterFunc , updateFunc , exitFunc);
    }

    public void Update(float dt)
    {
        if (_currentState != null)
        {
            _currentStateTime += dt;
            _currentState.Update(dt);
        }
    }

    public void SetState(FSMState state)
    {
        ///避免重复进入状态 状态列表不包含此状态
        if (state == _currentState || !_stateList.Contains(state))
            return;

        ///避免在A状态Exit执行B状态  B状态进入 A状态离开再次调用A状态的Exit
        if (_currentState != null && _lastExit != _currentState.Exit)
        {
            _lastExit = _currentState.Exit;
            _lastExit();
        }
            

        state.Enter();

        _lastState = _currentState;
        _currentState = state;
        _currentStateTime = 0;
    }

    public bool IsState(FSMState state)
    {
        return _currentState == state;
    }
}



public class FSMState
{
    string _path;
    public string path
    {
        get { return _path; }
    }

    DelegateOfVoid _enterFunc;
    DelegateOfUpdate _updateFunc;
    DelegateOfVoid _exitFunc;

    public FSMState(string path)
    {
        _path = path;
    }

    public void Begin(DelegateOfVoid enterFunc, DelegateOfUpdate updateFunc, DelegateOfVoid exitFunc)
    {
        _enterFunc = enterFunc;
        _updateFunc = updateFunc;
        _exitFunc = exitFunc;
    }

    public void Enter()
    {
        if (_enterFunc != null)
            _enterFunc();
    }

    public void Update(float dt)
    {
        if (_updateFunc != null)
            _updateFunc(dt);
    }

    public void Exit()
    {
        if (_exitFunc != null)
            _exitFunc();
    }
}
